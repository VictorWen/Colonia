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

        public UnitEntityGraphics(UnitEntityMovementGraphics moveGraphics, UnitEntityCombatGraphics combatGraphics)
        {
            this.MovementGraphics = moveGraphics;
            this.CombatGraphics = combatGraphics;
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
