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

        private TempUnitEntity unitEntity;

        public UnitEntityGraphics(TempUnitEntity unitEntity, UnitEntityMovementGraphics moveGraphics, UnitEntityCombatGraphics combatGraphics)
        {
            this.unitEntity = unitEntity;
            this.MovementGraphics = moveGraphics;
            this.CombatGraphics = combatGraphics;

            unitEntity.OnSelect += OnSelect;
            unitEntity.OnDeselect += OnDeselect;
            unitEntity.OnMoveAction += OnMoveAction;
            unitEntity.OnAttackAction += OnAttackAction;
        }

        public void OnSelect()
        {
            MovementGraphics.ShowSelectionIndicator();
        }

        public void OnDeselect()
        {
            MovementGraphics.HideSelectionIndicator();
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
