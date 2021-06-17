using System;
using System.Collections.Generic;
using UnityEngine;
using Tiles;

namespace Units.Abilities
{
    class HexAbilityAOE : AbilityAOE
    { 
        [SerializeField] private int range;

        public HexAbilityAOE() { }

        public HexAbilityAOE(int range)
        {
            this.range = range;
        }

        public override HashSet<Vector3Int> GetAOE(Vector3Int casterPos, Vector3Int targetPos, IWorld world)
        {
            return world.GetTilesInRange(targetPos, range);
        }

        public override string GetDescription()
        {
            return "Affects a circle around the target of radius " + range;
        }
    }
}
