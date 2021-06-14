using System;
using System.Collections.Generic;
using UnityEngine;
using Units.Abilities;

namespace Units.Combat
{
    public interface IUnitEntityCombat
    {
        int Mana { get; }

        int Attack { get; }
        int Piercing { get; }
        int Magic { get; }
        bool CanAttack { get; }
        UnitEntity Unit { get; }

        List<string> Abilities { get; }

        event Action OnAttack;

        void OnNextTurn(GameMaster game);

        void CastAbility(Ability ability, Vector3Int target);

        void BasicAttackOnPosition(Vector3Int position);

        void DealDamage(float baseDamage, IUnitEntityCombat attacker, bool isPhysicalDamage = true);

        bool IsEnemy(IUnitEntityCombat other);

        HashSet<Vector3Int> GetAttackables();

        string GetStatusDescription();
    }
}
