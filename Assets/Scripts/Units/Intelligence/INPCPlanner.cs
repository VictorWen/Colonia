using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units.Intelligence
{
    public interface INPCPlanner
    {
        // Determine both the desired target location and how to get there
        LinkedList<Vector3Int> GetMovementPath(UnitEntity self, Dictionary<Vector3Int, float> posScores);

        void ExecuteAbility(UnitEntity self, World world);
    }
}
