using System.Collections.Generic;
using Units.Abilities;
using UnityEngine;

namespace Units.Intelligence
{
    class SimpleMovementAI : NPCMovementAI
    {
        public override LinkedList<Vector3Int> GetAbilityMovement(UnitEntity self, Ability ability, Vector3Int target, World world)
        {
            return GetMinDistance(self.Position, self.MovementSpeed, target, world);
        }

        public override LinkedList<Vector3Int> GetRetreatMovement(UnitEntity self, Vector3Int target, World world)
        {
            return GetMinDistance(self.Position, self.MovementSpeed, target, world);
        }

        public override LinkedList<Vector3Int> GetWanderMovement(UnitEntity self, Vector3Int target, World world)
        {
            return GetMinDistance(self.Position, self.MovementSpeed, target, world);
        }

        private LinkedList<Vector3Int> GetMinDistance(Vector3Int start, int speed, Vector3Int target, World world)
        {
            BFSPathfinder bfs = new BFSPathfinder(start, speed, world, true);
            Vector3Int minDistance = start;
            foreach (Vector3Int tile in bfs.Reachables)
            {
                if (Vector3Int.Distance(target, tile) < Vector3Int.Distance(target, minDistance))
                {
                    minDistance = tile;
                }
            }
            return bfs.GetPathTo(minDistance);
        }
    }
}
