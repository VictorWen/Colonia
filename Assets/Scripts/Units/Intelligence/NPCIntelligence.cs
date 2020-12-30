﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Intelligence
{
    public interface NPCIntelligence
    {
        LinkedList<Vector3Int> GetMovement(UnitEntity self, Vector3Int target, World world);

        Vector3Int GetTarget(UnitEntity self, World world);
    }
}