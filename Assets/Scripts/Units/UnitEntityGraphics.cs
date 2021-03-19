using System;
using System.Collections.Generic;
using Units.Movement;
using Units.Combat;
using UnityEngine;

namespace Units
{
    public class UnitEntityGraphics
    {
        public UnitEntityMovementGraphics MovementGraphics { get; private set; }
        public UnitEntityCombatGraphics CombatGraphics { get; private set; }

        public bool ShowingSelectionIndicator { get; private set; }
        public BaseUnitEntity Unit { get; private set; }
        public UnitEntityConfig Config { get; private set; }
        public World World { get; private set; }
        public GameObject Obj { get; private set; }

        public UnitEntityGraphics(GameObject obj, BaseUnitEntity unit, UnitEntityConfig config, World world)
        {
            this.Obj = obj;
            ShowingSelectionIndicator = false;
            this.Unit = unit;
            this.Config = config;
            this.World = world;

            this.MovementGraphics = new UnitEntityMovementGraphics(this);
            this.CombatGraphics = new UnitEntityCombatGraphics(this);

            unit.OnDamaged += OnDamaged;
        }

        public void OnDamaged(int damage)
        {
            Debug.Log(Unit.Name + " damaged for " + damage);
        }

        public void OnSelect()
        {
            ShowSelectionIndicator();
        }

        public void OnDeselect()
        {
            HideSelectionIndicator();
        }

        public void ShowSelectionIndicator()
        {
            if (!ShowingSelectionIndicator)
            {
                Vector3Int spritePosition = World.grid.WorldToCell(Obj.transform.position);
                World.movement.SetTile(spritePosition, Config.selectTile);
                ShowingSelectionIndicator = true;
            }
        }

        public void HideSelectionIndicator()
        {
            if (ShowingSelectionIndicator)
            {
                Vector3Int spritePosition = World.grid.WorldToCell(Obj.transform.position);
                MovementGraphics.ClearMoveables();
                World.movement.SetTile(spritePosition, null);
                ShowingSelectionIndicator = false;
            }
        }

        public void OnMoveAction(HashSet<Vector3Int> moveables)
        {
            CombatGraphics.ClearAttackables();
            MovementGraphics.ShowMoveables(moveables);
        }

        public void OnAttackAction(HashSet<Vector3Int> attackables)
        {
            MovementGraphics.ClearMoveables();
            CombatGraphics.ShowAttackables(attackables);
        }
    }
}
