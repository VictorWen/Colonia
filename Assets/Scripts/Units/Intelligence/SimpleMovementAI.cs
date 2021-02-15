using System.Collections.Generic;
using Units.Abilities;
using Units.Movement;
using UnityEngine;

namespace Units.Intelligence
{
    class SimpleMovementAI : NPCMovementAI
    {
        public override LinkedList<Vector3Int> GetAbilityMovement(UnitEntityMovement self, Ability ability, Vector3Int target, World world)
        {
            return GetMinDistance(self, target);
        }

        public override LinkedList<Vector3Int> GetRetreatMovement(UnitEntityMovement self, Vector3Int target, World world)
        {
            return GetMinDistance(self, target);
        }

        public override LinkedList<Vector3Int> GetWanderMovement(UnitEntityMovement self, Vector3Int target, World world)
        {
            return GetMinDistance(self, target);
        }

        private LinkedList<Vector3Int> GetMinDistance(UnitEntityMovement movement, Vector3Int target)
        {
            BFSPathfinder bfs = movement.GetMoveables();
            Vector3Int minDistance = movement.Position;
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
