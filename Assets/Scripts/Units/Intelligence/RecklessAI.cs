using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Intelligence
{
    class RecklessAI : NPCIntelligence
    {
        public Vector3Int GetTarget(UnitEntity self, World world)
        {
            // Find Visible targets
            List<Vector3Int> targets = new List<Vector3Int>();
            foreach (Vector3Int gridPos in self.VisibleTiles)
            {
                if (world.UnitManager.Positions.ContainsKey(gridPos) && world.UnitManager.Positions[gridPos].PlayerControlled)
                    targets.Add(gridPos);
            }
            // Find the closest one
            if (targets.Count > 0) {
                int minIndex = 0;
                float minDistance = Vector3Int.Distance(targets[0], self.Position);
                for (int i = 1; i < targets.Count; i++)
                {
                    float dist = Vector3Int.Distance(targets[i], self.Position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        minIndex = i;
                    }
                }
                return targets[minIndex];
            }

            // Else, return random visible tile
            int index = world.RNG.Next(self.VisibleTiles.Count);
            int j = 0;
            foreach (Vector3Int visible in self.VisibleTiles)
            {
                if (j == index)
                    return visible;
                j++;
            }

            return self.Position;
        }

        public LinkedList<Vector3Int> GetMovement(UnitEntity self, Vector3Int target, World world)
        {
            /* Pseudocode:
             *  if target is an enemy
             *      add any visible and reachable position that can target the enemy as a candidate
             *      if there are no such positions
             *          try to move closer to the enemy: use djikstra's
             *  else
             *      use djikstra's to move towards the target location
             */

            if (world.UnitManager.Positions.ContainsKey(target))
            {
                // Find any position that the target is attackable from
                List<Vector3Int> candidates = new List<Vector3Int>();
                int attackRange = 1;
                foreach (Vector3Int withinRange in world.GetTilesInRange(target, attackRange))
                {
                    if (withinRange != target && world.IsReachable(self.MovementSpeed, withinRange, true) >= 0 && self.VisibleTiles.Contains(withinRange))
                        candidates.Add(withinRange);
                }

                // Select a random candidate
                int index = world.RNG.Next(candidates.Count);
                int i = 0;
                foreach (Vector3Int candidate in candidates)
                {
                    if (i == index)
                        target = candidate;
                    i++;
                }
            }
            // Move towards target
            DijkstraPathfinder dijkstra = new DijkstraPathfinder(self.Position, self.MovementSpeed, target, world, true);
            return dijkstra.NextMovement(self.MovementSpeed, world);
        }
    }
}
