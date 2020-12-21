using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Abilities
{
    class HexAbilityAOE : AbilityAOE
    {
        private readonly int range;

        public HexAbilityAOE(int range)
        {
            this.range = range;
        }

        public override Vector3Int[] GetAOE(Vector3Int casterPos, Vector3Int targetPos, World world)
        {
            Vector3Int[] aoe = new Vector3Int[range * (range + 1) * 3 + 1];
            world.GetTilesInRange(targetPos, range).CopyTo(aoe);
            return aoe;
        }

        public override string GetDescription()
        {
            return "Affects a circle around the target.\nRadius: " + range;
        }
    }
}
