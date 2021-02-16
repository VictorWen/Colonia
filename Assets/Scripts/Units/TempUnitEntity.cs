using System;
using Units.Movement;
using Units.Combat;
using System.Collections.Generic;
using Units.Abilities;
using UnityEngine;

namespace Units
{
    public class TempUnitEntity
    {
        [SerializeReference] private readonly UnitEntityMovement movement;
        [SerializeReference] private readonly UnitEntityCombat combat;

        private readonly UnitPanel panel;

        public event Action OnSelect;
        public event Action OnDeselect;
        public event Action<HashSet<Vector3Int>> OnMoveAction;
        public event Action<HashSet<Vector3Int>> OnAttackAction;

        public string Name { get; private set; }
        public bool IsSelected { get; private set; }
        public bool IsWaitingForMovement { get; private set; }
        public bool IsWaitingForAttack { get; private set; }
        public bool IsPlayerControlled { get; private set; }
        public UnitEntityMovement Movement { get { return movement; } }
        public UnitEntityCombat Combat { get { return combat; } }

        public TempUnitEntity(string name, bool isPlayerControlled, UnitEntityMovement movement, UnitEntityCombat combat, UnitPanel panel)
        {
            Name = name;
            IsPlayerControlled = isPlayerControlled;

            this.movement = movement;
            this.combat = combat;
            this.panel = panel;

            IsSelected = false;
        }

        public void Select()
        {
            IsSelected = true;
            panel.SetSelectedUnit(this);
            OnSelect?.Invoke();
        }

        public void Deselect()
        {
            IsSelected = false;
            panel.SetSelectedUnit(null);
            OnDeselect?.Invoke();
        }

        public void MoveAction()
        {
            IsWaitingForAttack = false;
            IsWaitingForMovement = true;
            OnMoveAction?.Invoke(Movement.GetMoveables().Reachables);
        }

        public void AttackAction()
        {
            IsWaitingForMovement = false;
            IsWaitingForAttack = true;
            OnAttackAction?.Invoke(Combat.GetAttackables());
        }

        public void OnMoveClick(Vector3Int gridPos)
        {
            Movement.MoveTo(gridPos);
            IsWaitingForMovement = false;
        }

        public void OnAttackClick(Vector3Int gridPos)
        {
            Combat.Attack(gridPos);
            IsWaitingForAttack = false;
        }
    }
}
