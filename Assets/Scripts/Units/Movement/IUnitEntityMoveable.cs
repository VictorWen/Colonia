using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void VisionAction(HashSet<Vector3Int> recon);

namespace Units.Movement
{
    public interface IUnitEntityMoveable
    {
        event Action OnMove;

        event VisionAction OnVisionUpdate;

        bool CanMove { get; }

        Vector3Int Position { get; }

        HashSet<Vector3Int> Visibles { get; }
    }
}
