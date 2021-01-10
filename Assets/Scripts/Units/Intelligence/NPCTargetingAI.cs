using UnityEngine;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating who to attack and/or where to move to
    /// </summary>
    public interface NPCTargetingAI
    {
        UnitEntity GetAttackTarget(UnitEntity self, World world);

        Vector3Int GetMovementTarget(UnitEntity self, UnitEntity attackTarget, World world);
    }
}