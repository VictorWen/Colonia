using System;
using System.Collections.Generic;
using UnityEngine;
using Tiles;

namespace Units.Abilities
{
    /// <summary>
    /// Defines the Area of Effect for an ability
    /// </summary>
    [Serializable]
    public abstract class AbilityAOE
    {
        public abstract HashSet<Vector3Int> GetAOE(Vector3Int casterPos, Vector3Int targetPos, IWorld world);

        public abstract string GetDescription();
    }
}
