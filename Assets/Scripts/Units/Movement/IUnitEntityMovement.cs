using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void VisionAction(HashSet<Vector3Int> recon);

namespace Units.Movement
{
    public interface IUnitEntityMovement
    {
        bool CanMove { get; set; }

        BFSPathfinder GetMoveables();

        void OnNextTurn(GameMaster game);
    }
}
