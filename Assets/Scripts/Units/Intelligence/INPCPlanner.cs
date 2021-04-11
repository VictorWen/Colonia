using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units.Intelligence
{
    public abstract class INPCPlanner : MonoBehaviour
    {
        // Determine both the desired target location and how to get there
        public abstract LinkedList<Vector3Int> GetMovementPath(UnitEntity self, Dictionary<Vector3Int, float> posScores);
    }
}
