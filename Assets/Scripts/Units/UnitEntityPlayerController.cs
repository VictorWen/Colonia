using Items;
using System.Collections.Generic;
using Units.Abilities;
using Units.Combat;
using Units.Intelligence;
using Units.Movement;
using UnityEngine;

namespace Units
{
    public class UnitEntityPlayerController : UnitEntityController
    {
        [SerializeField] private bool selected;

        [SerializeField] private UnitEntityConfig config;

        [SerializeReference] private UnitEntityGraphics graphics;

        public bool IsSelected { get; private set; }
        public bool IsWaitingForAttack { get; private set; }
        public bool IsWaitingForMovement { get; private set; }

        public BaseUnitEntity Unit { get { return unitEntity; } }

        private UnitPanelController panel;

        protected override void Awake()
        {
            Vector3Int gridPos = world.grid.WorldToCell(transform.position);
            transform.position = world.grid.CellToWorld(gridPos);

            // TODO: Placeholder unitEntity, should be constructed in the model
            unitEntity = new BaseUnitEntity(name, gridPos, 100, 4, world, true, 3);

            world.UnitManager.AddUnit(unitEntity);
            graphics = new UnitEntityGraphics(gameObject, unitEntity, config, world);

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
                    Select();
                else
                    Deselect();
            }
        }

        private void Update()
        {
            if (config.playerControlled && Input.GetMouseButtonUp(0) && (IsWaitingForMovement || IsWaitingForAttack))
            {
                Vector3 pos = gui.playerCam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(pos);
                OnTileClick(gridPos);
            }
        }

        public void Select()
        {
            gui.GUIState.SetState(GUIStateManager.UNIT);

            IsSelected = true;
            IsWaitingForMovement = false;
            IsWaitingForAttack = false;

            panel.SetSelectedUnit(this);
            graphics.OnSelect();
        }

        public void Deselect()
        {
            gui.GUIState.SetState(GUIStateManager.MAP);

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
            // TODO: fill in CastAbility
        }

        private void OnTileClick(Vector3Int tilePos)
        {
            if (IsWaitingForMovement && graphics.MovementGraphics.ShownMoveables.Contains(tilePos))
                unitEntity.MoveTo(tilePos);
            else if (IsWaitingForAttack && graphics.CombatGraphics.ShownAttackables.Contains(tilePos))
                unitEntity.Combat.BasicAttackOnPosition(tilePos);
        }

        


        /*    


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