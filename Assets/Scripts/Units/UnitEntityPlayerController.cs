using Units.Abilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Units
{
    public class UnitEntityPlayerController : UnitEntityController
    {
        public enum WaitingStatus {
            NONE, MOVEMENT, ATTACK, ABILITY
        }

        [SerializeField] private bool selected;
        [SerializeField] private UnitEntityConfig config = null;
        [SerializeReference] private UnitEntityGraphics graphics;

        public bool IsSelected { get; private set; }
        public WaitingStatus Status { get; private set; }

        //public UnitEntity Unit { get { return unitEntity; } }

        private UnitPanelController panel;

        protected override void Awake()
        {
            Vector3Int gridPos = world.grid.WorldToCell(transform.position);
            transform.position = world.grid.CellToWorld(gridPos);

            // TODO: Placeholder unitEntity, should be constructed in the model
            unitEntity = new UnitEntity(name, gridPos, 100, 4, world, true, 3);

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
            if (config.playerControlled && Input.GetMouseButtonUp(0) && (Status != WaitingStatus.NONE))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                Vector3 pos = gui.playerCam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = world.grid.WorldToCell(pos);
                OnTileClick(gridPos);
            }
        }

        public void Select()
        {
            gui.GUIState.SetState(GUIStateManager.UNIT);
            AllowHovering(false);

            IsSelected = true;
            Status = WaitingStatus.NONE;

            panel.SetSelectedUnit(this);
            graphics.OnSelect();
        }

        public void Deselect()
        {
            gui.GUIState.SetState(GUIStateManager.MAP);
            AllowHovering(true);

            IsSelected = false;

            panel.SetSelectedUnit(null);
            graphics.OnDeselect();
        }

        public void MoveAction() 
        {
            Status = WaitingStatus.MOVEMENT;
            graphics.OnMoveAction(unitEntity.Movement.GetMoveables().Reachables);
        }

        public void AttackAction()
        {
            Status = WaitingStatus.ATTACK;
            graphics.OnAttackAction(unitEntity.Combat.GetAttackables());
        }

        public void AbilityAction(Ability ability)
        {
            Status = WaitingStatus.ABILITY;
            AbilityCastTargeter targeter = new AbilityCastTargeter(ability, gui.GUIState, world, this, config);
            graphics.OnAbilityAction(targeter);
            StartCoroutine(targeter.TargetAndCastAbilityCoroutine());
        }

        private void OnTileClick(Vector3Int tilePos)
        {
            if (Status == WaitingStatus.MOVEMENT && graphics.MovementGraphics.ShownMoveables.Contains(tilePos))
                unitEntity.MoveTo(tilePos);
            else if (Status == WaitingStatus.ATTACK && graphics.CombatGraphics.ShownAttackables.Contains(tilePos))
                unitEntity.Combat.BasicAttackOnPosition(tilePos);
        }

        /*     public void OnDeath()
               {
                   //if (hovering)
                       //gui.unitPanel.HideUnitInfo();
               }

       */
        /*        public void ShowInventory()
                {
                    //gui.unitPanel.heroInventory.Enable(this);
                }*/
    }
}