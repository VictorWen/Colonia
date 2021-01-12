using Units.Abilities;
using UnityEngine;

namespace Units.Intelligence
{
    /// <summary>
    /// Determines who or where to target.
    /// </summary>
    public interface NPCTargetingAI
    {
        Vector3Int GetAbilityTarget(UnitEntity self, Ability ability, World world);

        Vector3Int GetManeuverTarget(UnitEntity self, Ability ability, Vector3Int abilityTarget, World world);

        Vector3Int GetRetreatTarget(UnitEntity self, World world);

        Vector3Int GetWanderTarget(UnitEntity self, World world);
    }
}
