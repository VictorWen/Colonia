using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units.Intelligence
{
    public class SimplePlanner : INPCPlanner
    {
        public override LinkedList<Vector3Int> GetMovementPath(UnitEntity self, Dictionary<Vector3Int, float> posScores)
        {
            List<KeyValuePair<Vector3Int, float>> scores = posScores.ToList();
            scores.Sort(PositioningCompare);
            Debug.Log(scores[0] + " " + scores[scores.Count - 1]);
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
    }
}
