using System;
using System.Collections.Generic;
using Units.Movement;
using Units.Combat;
using UnityEngine;
using UnityEngine.Tilemaps;
using Units.Abilities;

namespace Units
{
    public class UnitEntityGraphics
    {
        public UnitEntityMovementGraphics MovementGraphics { get; private set; }
        public UnitEntityCombatGraphics CombatGraphics { get; private set; }

        public bool ShowingSelectionIndicator { get; private set; }
        public UnitEntity Unit { get; private set; }
        public UnitEntityConfig Config { get; private set; }
        public World World { get; private set; }
        public GameObject Obj { get; private set; }

        private AbilityCastTargeter currentTargeter;

        public UnitEntityGraphics(GameObject obj, UnitEntity unit, UnitEntityConfig config, World world)
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
                Vector3Int spritePosition = World.WorldToCell(Obj.transform.position);
                World.SetMovementTile(spritePosition, Config.selectTile);
                ShowingSelectionIndicator = true;
            }
        }

        public void HideSelectionIndicator()
        {
            if (ShowingSelectionIndicator)
            {
                Vector3Int spritePosition = World.WorldToCell(Obj.transform.position);
                MovementGraphics.ClearMoveables();
                World.SetMovementTile(spritePosition, null);
                ShowingSelectionIndicator = false;
            }
        }

        public void OnMoveAction(HashSet<Vector3Int> moveables)
        {
            ClearTiles();
            MovementGraphics.ShowMoveables(moveables);
        }

        public void OnAttackAction(HashSet<Vector3Int> attackables)
        {
            ClearTiles();
            CombatGraphics.ShowAttackables(attackables);
        }

        public void OnAbilityAction(AbilityCastTargeter targeter)
        {
            ClearTiles();
            currentTargeter = targeter;
        }

        public void ClearTiles()
        {
            CombatGraphics.ClearAttackables();
            MovementGraphics.ClearMoveables();
            if (currentTargeter != null && currentTargeter.IsActive)
            {
                currentTargeter.Disable();
            }
        }
    }
}
