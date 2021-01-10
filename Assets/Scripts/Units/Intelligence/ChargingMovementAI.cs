using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Intelligence
{
    class ChargingMovementAI : NPCMovementAI
    {
        public LinkedList<Vector3Int> GetMovementAction(UnitEntity self, Vector3Int target, World world)
        {
            // Move towards target with shortest path
            DijkstraPathfinder dijkstra = new DijkstraPathfinder(self.Position, self.MovementSpeed, target, world, true);
            return dijkstra.NextMovement(self.MovementSpeed, world);
        }
    }
}
