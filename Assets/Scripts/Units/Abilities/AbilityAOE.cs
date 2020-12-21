using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Abilities
{
    /// <summary>
    /// Defines the Area of Effect for an ability
    /// </summary>
    public abstract class AbilityAOE
    {
        public abstract Vector3Int[] GetAOE(Vector3Int casterPos, Vector3Int targetPos, World world);

        public abstract string GetDescription();
    }
}
