using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    public class SimplePlanner : INPCPlanner
    {
        public override LinkedList<Vector3Int> GetMovementPath(UnitEntity self, Dictionary<Vector3Int, float> posScores)
        {
            List<KeyValuePair<Vector3Int, float>> scores = posScores.ToList();
            scores.Sort(PositioningCompare);
            return self.Movement.GetMoveables().GetPathTo(scores[0].Key);
        }

        private int PositioningCompare(KeyValuePair<Vector3Int, float> pair1, KeyValuePair<Vector3Int, float> pair2)
        {
            if (pair1.Value > pair2.Value)
                return -1;
            else if (pair1.Value < pair2.Value)
                return 1;
            else
                return 0;
        }

        public override void ExecuteAbility(UnitEntity self, World world)
        {
            List<string> abilities = self.Combat.Abilities;
            if (abilities.Contains("attack"))
            {
                foreach (Vector3Int tile in world.GetAdjacents(self.Position))
                {
                    if (world.UnitManager.GetUnitAt<UnitEntity>(tile) != null)
                    {
                        self.Combat.CastAbility(GlobalAbilityDictionary.GetAbility("attack"), tile);
                        break;
                    }
                }
            }
        }
    }
}
