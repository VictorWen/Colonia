using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating how the UnitEntity should move to a target location
    /// </summary>
    public interface NPCMovementAI
    {
        //TODO: convert NPCIntelligence.GetMovement to void 
        LinkedList<Vector3Int> GetMovementAction(UnitEntity self, Vector3Int target, World world);
    }
}