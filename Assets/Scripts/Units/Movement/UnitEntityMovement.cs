using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Movement
{
    public class UnitEntityMovement : IUnitEntityMoveable
    {
        [SerializeField] private Vector3Int position;
        [SerializeField] private int movementSpeed = 3;
        [SerializeField] private int sight = 4;

        public event Action OnMove;
        //public delegate void VisionAction(HashSet<Vector3Int> recon);
        public event VisionAction OnVisionUpdate;

        public Vector3Int Position { get { return position; } }
        public bool CanMove { get; private set; }
        public HashSet<Vector3Int> Visibles { get; private set; }

        private readonly World world;

        public UnitEntityMovement(Vector3Int initialPosition, World world)
        {
            this.world = world;
            position = initialPosition;
            CanMove = true;
            //world.AddUnitEntity(this);
        }

        public void MoveTo(Vector3Int destination)
        {
            CanMove = false;
            world.UnitManager.UpdateUnitPosition(position, destination);
            position = destination;
            OnMove?.Invoke();
            UpdateVision();
        }

        public BFSPathfinder GetMoveables()
        {
            BFSPathfinder pathfinder = new BFSPathfinder(position, movementSpeed, world, true);
            pathfinder.Reachables.Remove(position);
            return pathfinder;
        }

        public void UpdateVision()
        {
            Visibles = world.GetLineOfSight(position, sight);
            BFSPathfinder recon = new BFSPathfinder(position, movementSpeed, world);
            OnVisionUpdate?.Invoke(recon.Reachables);
        }

        public void OnNextTurn(GameMaster game)
        {
            CanMove = true;
        }
    }
}