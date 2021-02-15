using System;
using UnityEngine;

namespace Units.Combat
{
    public class UnitEntityDeath
    {
        public UnitEntityDeath(UnitEntityCombat combat)
        {
            combat.OnDeath += OnDeath;
        }

        public void OnDeath()
        {

        }
    }
}