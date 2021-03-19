﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Tiles;

namespace Units.Combat
{
    public interface IUnitEntityCombat
    {
        int Attack { get; }
        int Piercing { get; }
        int Magic { get; }
        bool CanAttack { get; }
        BaseUnitEntity Unit { get; }

        List<string> Abilities { get; }

        event Action OnAttack;

        void OnNextTurn(GameMaster game);

        void BasicAttackOnPosition(Vector3Int position);

        void DealDamage(float baseDamage, IUnitEntityCombat attacker, bool isPhysicalDamage = true);

        bool IsEnemy(IUnitEntityCombat other);

        HashSet<Vector3Int> GetAttackables();

        string GetStatusDescription();
    }
}