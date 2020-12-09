using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public interface NPCIntelligence
    {
        LinkedList<Vector3Int> GetMovement(UnitEntity self, Vector3Int target, World world);

        Vector3Int GetTarget(UnitEntity self, List<UnitEntity> others, World world);
    }
}