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
        // TODO: REMOVE OLD STUFF ========
        public UnitEntity Unit { get; set; }
        public void CastAbility(Ability ability)
        {
            //gui.unitPanel.HideAbilityMenu();
            //StartCoroutine(CastAbilityCoroutine(ability));
        }
        // ===============================

        [SerializeField] private GUIMaster gui;
        [SerializeField] private World world;

        [SerializeField] private bool selected;

        [SerializeField] private UnitEntityConfig config;

        [SerializeReference] private TempUnitEntity unitEntity;
        [SerializeReference] private UnitEntityGraphics graphics;

        private NPCUnitEntityAI ai;

        private void Awake()
        {
            Vector3Int gridPos = world.grid.WorldToCell(transform.position);
            transform.position = world.grid.CellToWorld(gridPos);

            UnitEntityMovement movement = new UnitEntityMovement(gridPos, world);
            UnitEntityCombat combat = new UnitEntityCombat(world, movement, config.playerControlled);
            UnitEntityDeath death = new UnitEntityDeath(combat);
            unitEntity = new TempUnitEntity(name, config.playerControlled, movement, combat, gui.unitPanel.UnitPanel);

            if (!config.playerControlled)
            {
                ai = new NPCUnitEntityAI(movement, combat, new RecklessAI(), new BasicTargettingAI(), new SimpleMovementAI());
                world.UnitManager.AddUnit(movement, combat, ai);
            }
            else
            {
                world.UnitManager.AddUnit(movement, combat);
            }

            UnitEntityMovementGraphics movementGraphics = new UnitEntityMovementGraphics(world, gameObject, movement, config);
            UnitEntityCombatGraphics combatGraphics = new UnitEntityCombatGraphics(world, combat, movement, config);
            graphics = new UnitEntityGraphics(unitEntity, movementGraphics, combatGraphics);
        }

        private void Start()
        {
            unitEntity.Movement.UpdateVision();
        }

        private void OnMouseOver()
        {
            if (gui.GUIState.UnitControl && config.playerControlled && Input.GetMouseButtonUp(0))
            {
                if (!unitEntity.IsSelected)
                {
                    gui.GUIState.SetState(GUIStateManager.UNIT);
                    unitEntity.Select();
                }
                else
                {
                    gui.GUIState.SetState(GUIStateManager.MAP);
                    unitEntity.Deselect();
                }
            }
        }

        private void Update()
        {
            if (config.playerControlled && Input.GetMouseButtonUp(0) && (unitEntity.IsWaitingForMovement || unitEntity.IsWaitingForAttack))
            {
                Vector3 pos = gui.playerCam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(pos);

                if (unitEntity.IsWaitingForMovement && graphics.MovementGraphics.ShownMoveables.Contains(gridPos))
                {
                    unitEntity.OnMoveClick(gridPos);
                    graphics.MovementGraphics.ClearMoveables();
                }

                if (unitEntity.IsWaitingForAttack && graphics.CombatGraphics.ShownAttackables.Contains(gridPos))
                {
                    unitEntity.OnAttackClick(gridPos);
                    graphics.CombatGraphics.ClearAttackables();
                }
            }
        }



 /*    

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