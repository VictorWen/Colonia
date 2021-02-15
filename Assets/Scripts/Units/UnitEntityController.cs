using System.Collections.Generic;
using Units.Abilities;
using Units.Combat;
using Units.Intelligence;
using Units.Movement;
using UnityEngine;

namespace Units
{
    public class UnitEntityController : MonoBehaviour
    {
        [SerializeField] private GUIMaster gui;
        [SerializeField] private World world;

        [SerializeField] private bool selected;

        [SerializeField] private UnitEntityConfig config;

        [SerializeReference] private UnitEntityMovement movement;
        [SerializeReference] private UnitEntityCombat combat;
        [SerializeReference] private UnitEntityDeath death;

        [SerializeReference] private UnitEntityMovementGraphics movementGraphics;
        [SerializeReference] private UnitEntityCombatGraphics combatGraphics;

        private bool waitingForMovement;
        private bool waitingForAttack;
        private HashSet<Vector3Int> moveables;
        private HashSet<Vector3Int> attackables;

        public UnitEntityCombat Combat { get { return combat; } }
        public UnitEntityMovement Movement { get { return movement; } }

        private NPCUnitEntityAI ai;

        private void Awake()
        {
            Vector3Int gridPos = world.grid.WorldToCell(transform.position);
            transform.position = world.grid.CellToWorld(gridPos);

            movement = new UnitEntityMovement(gridPos, world);
            combat = new UnitEntityCombat(world, movement, config.playerControlled);
            death = new UnitEntityDeath(combat);

            if (!config.playerControlled)
            {
                ai = new NPCUnitEntityAI(movement, combat, new RecklessAI(), new BasicTargettingAI(), new SimpleMovementAI());
                world.UnitManager.AddUnit(movement, combat, ai);
            }
            else
            {
                world.UnitManager.AddUnit(movement, combat);
            }

            movementGraphics = new UnitEntityMovementGraphics(world, gameObject, movement, config);
            combatGraphics = new UnitEntityCombatGraphics(world, combat, movement, config);
        }

        private void Start()
        {
            movement.UpdateVision();
        }

        private void OnMouseOver()
        {
            if (gui.GUIState.UnitControl && config.playerControlled && Input.GetMouseButtonUp(0))
            {
                if (!selected)
                {
                    gui.unitPanel.SetSelectedUnit(this);
                    gui.GUIState.SetState(GUIStateManager.UNIT);
                    selected = true;
                    movementGraphics.ShowSelectionIndicator();
                    //gui.unitPanel.SetSelectedUnit(this);
                }
                else
                {
                    gui.unitPanel.SetSelectedUnit(null);
                    gui.GUIState.SetState(GUIStateManager.MAP);
                    selected = false;
                    movementGraphics.HideSelectionIndicator();
                }
            }
        }

        private void Update()
        {
            if (config.playerControlled && Input.GetMouseButtonUp(0))
            {
                Vector3 pos = gui.playerCam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(pos);

                if (waitingForMovement && moveables.Contains(gridPos))
                {
                    movement.MoveTo(gridPos);
                    waitingForMovement = false;
                }

                if (waitingForAttack && attackables.Contains(gridPos))
                {
                    combat.Attack(gridPos);
                    waitingForAttack = false;
                }
            }
        }

        public void MoveAction()
        {
            if (waitingForAttack)
            {
                combatGraphics.ClearAttackables();
                waitingForAttack = false;
            }

            moveables = movement.GetMoveables().Reachables;
            movementGraphics.ShowMoveables(moveables);
            waitingForMovement = true;
        }

        public void AttackAction()
        {
            if (waitingForMovement)
            {
                movementGraphics.ClearMoveables();
                waitingForMovement = false;
            }

            attackables = combat.GetAttackables();
            combatGraphics.ShowAttackables(attackables);
            waitingForAttack = true;
        }

        //TODO: change to readonly
        public UnitEntity Unit { get; set;}

 /*       private void OnMouseOver()
        {
            if (gui.GUIState.UnitControl)
            {
                if (Unit.PlayerControlled && Input.GetMouseButtonUp(0))
                {
                    if (!selected)
                    {
                        //MoveAction();
                        //gui.unitPanel.SetSelectedUnit(this);
                        world.movement.SetTile(world.grid.WorldToCell(transform.position), gold);
                        gui.GUIState.SetState(GUIStateManager.UNIT);
                        selected = true;
                    }
                    else
                    {
                        ClearMovables();
                        ClearAttackables();
                        gui.unitPanel.SetSelectedUnit(null);
                        world.movement.SetTile(world.grid.WorldToCell(transform.position), null);
                        gui.GUIState.SetState(GUIStateManager.MAP);
                        selected = false;
                    }
                }
            }
        }

        private void OnMouseEnter()
        {
            //TODO: fix second unit panel call
            if (gui.GUIState.UnitControl)
            {
                //gui.unitPanel.ShowUnitInfo(Unit);
                hovering = true;
            }
        }

        private void OnMouseExit()
        {
            if (hovering)
            {
                //gui.unitPanel.HideUnitInfo();
                hovering = false;
            }
        }

        private void Update() 
        {
            if (selected && Input.GetMouseButtonUp(0))
            {
                Vector3 pos = gui.playerCam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(pos);
                if (moveablePos.Contains(gridPos))
                {
                    world.movement.SetTile(Unit.Position, null);
                    ClearMovables();
                    world.movement.SetTile(gridPos, gold);
                    Unit.MoveTo(gridPos, world);
                    //gui.UpdateAllUnitVisibilities();
                }
                if (attackablePos.Contains(gridPos))
                {
                    ClearAttackables();
                    Unit.AttackUnitEntity(world.GetUnitAt(gridPos), world);
                }
            }
        }

        public void OnDeath()
        {
            //if (hovering)
                //gui.unitPanel.HideUnitInfo();
        }

*/

        public void CastAbility(Ability ability)
        {
            //gui.unitPanel.HideAbilityMenu();
            //StartCoroutine(CastAbilityCoroutine(ability));
        }

/*        public void ShowInventory()
        {
            //gui.unitPanel.heroInventory.Enable(this);
        }*/

/*        private IEnumerator CastAbilityCoroutine(Ability ability)
        {
            HashSet<Vector3Int> range = ability.GetWithinRange(Unit, world);
            bool hasSelected = false;
            while (!hasSelected)
            {
                yield return null;
                world.movement.ClearAllTiles();

                // Detect right-click to cancel
                if (Input.GetMouseButtonUp(1))
                {
                    break;
                }
                
                // Get mouse positions
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(worldPos);
                // Check if in the Ability's range
                if (range.Contains(gridPos))
                {
                    // Highlight AOE
                    HashSet<Vector3Int> area = ability.GetAreaOfEffect(Unit.Position, gridPos, world);
                    foreach (Vector3Int tile in area)
                    {
                        world.movement.SetTile(tile, red);
                    }

                    // Detect mouse click
                    if (Input.GetMouseButtonUp(0))
                    {
                        Unit.CastAbility(ability, gridPos, world);
                        hasSelected = true;
                    }
                }
            }
            world.movement.ClearAllTiles();
            world.movement.SetTile(Unit.Position, gold);
            UpdateGraphics();
            yield break;
        }*/

    }
}