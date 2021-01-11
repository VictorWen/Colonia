
using System.Collections.Generic;
using UnityEngine;

namespace Units.Intelligence
{
    public class RecklessAI : NPCMainAI
    {
        public AIState GetState(NPCUnitEntity self, World world)
        {
            throw new System.NotImplementedException();
            /*if (attackTarget != null) // There is an enemy to attack
            {
                // Find all tiles that are within the attack range to attack the target UnitEntity
                int attackRange = 1; //TODO: PLACEHOLDER attack range for UnitEntity
                HashSet<Vector3Int> targetCandidates = world.GetTilesInRange(attackTarget.Position, attackRange);
                targetCandidates.Remove(attackTarget.Position);

                // Find the closet target candidate
                Vector3Int minDistance = attackTarget.Position;
                foreach (Vector3Int target in targetCandidates)
                {
                    if (minDistance.Equals(attackTarget.Position)
                        || Vector3Int.Distance(self.Position, target) < Vector3Int.Distance(self.Position, minDistance)
                        && world.IsReachable(self.MovementSpeed, target, true) >= 0)
                    {
                        minDistance = target;
                    }
                }

                // Edge case, no candidates are reachable
                if (minDistance.Equals(attackTarget.Position))
                    return self.Position;

                return minDistance;
            }
            else // There is no enemy => roam
            {
                // Move to a random reachable tile
                BFSPathfinder pathfinder = new BFSPathfinder(self.Position, self.MovementSpeed, world, true);
                int randIndex = world.RNG.Next(pathfinder.Reachables.Count);
                int i = 0;
                foreach (Vector3Int reachable in pathfinder.Reachables)
                {
                    if (randIndex == i)
                    {
                        return reachable;
                    }
                    i++;
                }

                return self.Position; // If no reachable tiles, stay put
            }*/
        }
    }
}
