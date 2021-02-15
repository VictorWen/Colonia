using System.Collections.Generic;
using Units.Abilities;
using Units.Movement;
using UnityEngine;

namespace Units.Intelligence
{
/*    class DijkstrasMovementAI : NPCMovementAI
    {
        public override LinkedList<Vector3Int> GetAbilityMovement(UnitEntityMovement self, Ability ability, Vector3Int target, World world)
        {
            return Dijkstras(self, target, world);
        }

        public override LinkedList<Vector3Int> GetRetreatMovement(UnitEntityMovement self, Vector3Int target, World world)
        {
            return Dijkstras(self, target, world);
        }

        public override LinkedList<Vector3Int> GetWanderMovement(UnitEntityMovement self, Vector3Int target, World world)
        {
            return Dijkstras(self, target, world);
        }

        private LinkedList<Vector3Int> Dijkstras(UnitEntityMovement self, Vector3Int target, World world)
        {
            DijkstraPathfinder dijkstra = new DijkstraPathfinder(self.Position, self.MovementSpeed, target, world, true);
            return dijkstra.NextMovement(self.MovementSpeed, world);
        }
    }*/
}
