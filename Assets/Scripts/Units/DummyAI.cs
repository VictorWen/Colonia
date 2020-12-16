using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    ///<summary>
    ///This a very stupid AI.
    ///Only use for testing or as a template.
    ///</summary>
    class DummyAI : NPCIntelligence
    {
        public Vector3Int GetTarget(UnitEntity self, World world)
        {
            foreach (Vector3Int gridPos in self.VisibleTiles)
            {
                if (world.UnitManager.Positions.ContainsKey(gridPos) && world.UnitManager.Positions[gridPos].PlayerControlled)
                    return gridPos;
            }

            // Default case:
            int index = world.RNG.Next(self.VisibleTiles.Count);
            Debug.Log(self.VisibleTiles.Count);
            int i = 0;
            foreach (Vector3Int visible in self.VisibleTiles)
            {
                if (i == index)
                    return visible;
                i++;
            }
            return self.Position;
        }

        public LinkedList<Vector3Int> GetMovement(UnitEntity self, Vector3Int target, World world)
        {
            // *Find destination candidates*
            List<Vector3Int> candidates = new List<Vector3Int>();
            int attackRange = 1;
            // Does not account for line of sight or vision in general
            PathfinderBFS pathfinder = new PathfinderBFS(self.Position, self.MovementSpeed, world, true);
            foreach (Vector3Int withinRange in world.GetTilesInRange(target, attackRange))
            {
                if (withinRange != target && pathfinder.Reachables.Contains(withinRange))
                    candidates.Add(withinRange);
            }

            // *Insert candidate analysis*
            // random candidate analysis
            int index = world.RNG.Next(candidates.Count);
            int i = 0;
            foreach (Vector3Int candidate in candidates)
            {
                if (i == index)
                    return pathfinder.GetPathTo(candidate);
                i++;
            }
            return pathfinder.GetPathTo(self.Position);
        }
    }
}
