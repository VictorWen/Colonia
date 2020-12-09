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

        public Vector3Int GetTarget(UnitEntity self, List<UnitEntity> entities, World world)
        {
            HashSet<Vector3Int> entityPositions = new HashSet<Vector3Int>();
            foreach (UnitEntity entity in entities)
            {
                entityPositions.Add(entity.Position);
            }

            PathfinderBFS pathfinder = new PathfinderBFS(self.Position, self.MovementSpeed, world);

            foreach (Vector3Int gridPos in pathfinder.Reachables)
            {
                if (entityPositions.Contains(gridPos))
                    return gridPos;
            }

            return self.Position;
        }

        public LinkedList<Vector3Int> GetMovement(UnitEntity self, Vector3Int target, World world)
        {
            List<Vector3Int> candidates = new List<Vector3Int>();
            int attackRange = 1;
            // Does not account for line of sight or vision in general
            PathfinderBFS pathfinder = new PathfinderBFS(self.Position, self.MovementSpeed, world, true);
            foreach (Vector3Int withinRange in world.GetTilesInRange(target, attackRange))
            {
                //world.movement.SetTile(withinRange, world.cloud);
                if (withinRange != target && pathfinder.Reachables.Contains(withinRange))
                    candidates.Add(withinRange);
            }

            // *Insert candidate analysis*

            //placeholder
            Debug.Log("Motion Target: " + candidates[0]);
            return pathfinder.GetPathTo(candidates[0]);
        }
    }
}
