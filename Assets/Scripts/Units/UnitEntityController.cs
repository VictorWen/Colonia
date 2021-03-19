using Items;
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

        [SerializeReference] private BaseUnitEntity unitEntity;
        [SerializeReference] private UnitEntityGraphics graphics;

        private NPCUnitEntityAI ai;

        public bool IsSelected { get; private set; }
        public bool IsWaitingForAttack = false;
        public bool IsWaitingForMovement = true;

        public BaseUnitEntity Unit { get { return unitEntity; } }

        private UnitPanelController panel;

        private void Awake()
        {
            Vector3Int gridPos = world.grid.WorldToCell(transform.position);
            transform.position = world.grid.CellToWorld(gridPos);

            unitEntity = new BaseUnitEntity(name, gridPos, 100, 4, world, config.playerControlled, 3);

            world.UnitManager.AddUnit(unitEntity);

            UnitEntityMovementGraphics movementGraphics = new UnitEntityMovementGraphics(world, gameObject, unitEntity, config);
            UnitEntityCombatGraphics combatGraphics = new UnitEntityCombatGraphics(world, unitEntity.Combat, unitEntity.Movement, config);
            graphics = new UnitEntityGraphics(movementGraphics, combatGraphics);

            panel = gui.unitPanel;
        }

        private void Start()
        {
            unitEntity.UpdateVision();
        }

        private void OnMouseOver()
        {
            if (gui.GUIState.UnitControl && config.playerControlled && Input.GetMouseButtonUp(0))
            {
                if (!IsSelected)
                {
                    gui.GUIState.SetState(GUIStateManager.UNIT);
                    Select();
                }
                else
                {
                    gui.GUIState.SetState(GUIStateManager.MAP);
                    Deselect();
                }
            }
        }

        private void Update()
        {
            if (config.playerControlled && Input.GetMouseButtonUp(0) && (IsWaitingForMovement || IsWaitingForAttack))
            {
                Vector3 pos = gui.playerCam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(pos);

                if (IsWaitingForMovement && graphics.MovementGraphics.ShownMoveables.Contains(gridPos))
                {
                    unitEntity.MoveTo(gridPos);
                }

                if (IsWaitingForAttack && graphics.CombatGraphics.ShownAttackables.Contains(gridPos))
                {
                    //unitEntity.OnAttackClick(gridPos);
                }
            }
        }

        public void Select()
        {
            IsSelected = true;
            panel.SetSelectedUnit(this);
            graphics.OnSelect();
        }

        public void Deselect()
        {
            IsSelected = false;
            panel.SetSelectedUnit(null);
            graphics.OnDeselect();
        }

        public void MoveAction() 
        {
            IsWaitingForMovement = true;
            IsWaitingForAttack = false;
            graphics.OnMoveAction(unitEntity.Movement.GetMoveables().Reachables);
        }

        public void AttackAction()
        {
            IsWaitingForMovement = false;
            IsWaitingForAttack = true;
            graphics.OnAttackAction(unitEntity.Combat.GetAttackables());
        }

        public void CastAbility(Ability ability)
        {

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