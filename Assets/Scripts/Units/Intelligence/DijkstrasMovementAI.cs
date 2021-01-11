using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Intelligence
{
    class DijkstrasMovementAI : NPCMovementAI
    {
        public LinkedList<Vector3Int> GetAbilityMovement(UnitEntity self, UnitEntity target, World world)
        {
            return Dijkstras(self, target.Position, world);
        }

        public LinkedList<Vector3Int> GetRetreatMovement(UnitEntity self, Vector3Int target, World world)
        {
            return Dijkstras(self, target, world);
        }

        public Vector3Int GetRetreatTarget(UnitEntity self, World world)
        {
            throw new NotImplementedException();
        }

        public LinkedList<Vector3Int> GetWanderMovement(UnitEntity self, Vector3Int target, World world)
        {
            return Dijkstras(self, target, world);
        }

        public Vector3Int GetWanderTarget(UnitEntity self, World world)
        {
            throw new NotImplementedException();
        }

        private LinkedList<Vector3Int> Dijkstras(UnitEntity self, Vector3Int target, World world)
        {
            DijkstraPathfinder dijkstra = new DijkstraPathfinder(self.Position, self.MovementSpeed, target, world, true);
            return dijkstra.NextMovement(self.MovementSpeed, world);
        }
    }
}
