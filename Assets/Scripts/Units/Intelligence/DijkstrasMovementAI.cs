using System;
using System.Collections.Generic;
using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    class DijkstrasMovementAI : NPCMovementAI
    {
        public override LinkedList<Vector3Int> GetAbilityMovement(UnitEntity self, Ability ability, Vector3Int target, World world)
        {
            return Dijkstras(self, target, world);
        }

        public override LinkedList<Vector3Int> GetRetreatMovement(UnitEntity self, Vector3Int target, World world)
        {
            return Dijkstras(self, target, world);
        }

        public override LinkedList<Vector3Int> GetWanderMovement(UnitEntity self, Vector3Int target, World world)
        {
            return Dijkstras(self, target, world);
        }

        private LinkedList<Vector3Int> Dijkstras(UnitEntity self, Vector3Int target, World world)
        {
            DijkstraPathfinder dijkstra = new DijkstraPathfinder(self.Position, self.MovementSpeed, target, world, true);
            return dijkstra.NextMovement(self.MovementSpeed, world);
        }
    }
}
