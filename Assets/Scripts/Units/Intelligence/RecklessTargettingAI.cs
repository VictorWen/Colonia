
using UnityEngine;

using System.Collections.Generic;

namespace Units.Intelligence
{
    public class RecklessTargettingAI : NPCTargetingAI
    {
        public UnitEntity GetAttackTarget(UnitEntity self, World world)
        {
            // Find all visible enemey UnitEntities
            List<UnitEntity> targets = new List<UnitEntity>();
            foreach (Vector3Int visible in self.VisibleTiles)
            {
                if (world.UnitManager.Positions.ContainsKey(visible) && world.UnitManager.Positions[visible].PlayerControlled)
                {
                    targets.Add(world.UnitManager.Positions[visible]);
                }
            }

            // Target candidate analysis, determine best UnitEntity to target
            // Find the closest UnitEntity
            UnitEntity minDistance = null;
            foreach (UnitEntity target in targets)
            {
                if (minDistance == null || Vector3Int.Distance(self.Position, target.Position) < Vector3Int.Distance(self.Position, minDistance.Position))
                {
                    minDistance = target;
                }
            }
            return minDistance;
        }

        public Vector3Int GetMovementTarget(UnitEntity self, UnitEntity attackTarget, World world)
        {
            if (attackTarget != null) // There is an enemy to attack
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
            }
        }
    }
}
