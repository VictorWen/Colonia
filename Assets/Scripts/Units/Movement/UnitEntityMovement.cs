using System;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Units.Movement
{
    public class UnitEntityMovement : IUnitEntityMovement
    {
        [SerializeField] private int movementSpeed = 3;

        private readonly BaseUnitEntity unit;
        private readonly IWorld world;

        public bool CanMove { get; set; }

        public UnitEntityMovement(BaseUnitEntity unit, IWorld world, int movementSpeed)
        {
            this.unit = unit;
            this.world = world;
            this.movementSpeed = movementSpeed;
            
            CanMove = true;
        }

        public BFSPathfinder GetMoveables()
        {
            BFSPathfinder pathfinder = new BFSPathfinder(unit.Position, movementSpeed, world, true);
            pathfinder.Reachables.Remove(unit.Position);
            return pathfinder;
        }

        public void OnNextTurn(GameMaster game)
        {
            CanMove = true;
        }
    }
}