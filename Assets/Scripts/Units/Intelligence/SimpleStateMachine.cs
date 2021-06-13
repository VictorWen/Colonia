using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units.Intelligence
{
    public class SimpleStateMachine : INPCStateMachine
    {
        public AIState GetState(UnitEntity self, World world)
        {
            bool found = false;
            foreach (Vector3Int visible in self.Visibles)
            {
                UnitEntity unitAt = world.UnitManager.GetUnitAt<UnitEntity>(visible);
                if (unitAt != null && unitAt.Combat.IsEnemy(self.Combat))
                {
                    found = true;
                    break;
                }
            }

            return found ? AIState.COMBAT : AIState.WANDER;
        }
    }
}
