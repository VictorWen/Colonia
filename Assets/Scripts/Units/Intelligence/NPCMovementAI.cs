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
        LinkedList<Vector3Int> GetAbilityMovement(UnitEntity self, UnitEntity target, World world);

        Vector3Int GetRetreatTarget(UnitEntity self, World world);

        LinkedList<Vector3Int> GetRetreatMovement(UnitEntity self, Vector3Int target, World world);

        Vector3Int GetWanderTarget(UnitEntity self, World world);

        LinkedList<Vector3Int> GetWanderMovement(UnitEntity self, Vector3Int target, World world);
    }
}