using Units.Abilities;
using UnityEngine;

namespace Units.Intelligence
{
    /// <summary>
    /// Determines who or where to target.
    /// </summary>
    public abstract class NPCTargetingAI : MonoBehaviour
    {
        public abstract Vector3Int GetAbilityTarget(UnitEntity self, Ability ability, World world);

        public abstract Vector3Int GetManeuverTarget(UnitEntity self, Ability ability, Vector3Int abilityTarget, World world);

        public abstract Vector3Int GetRetreatTarget(UnitEntity self, World world);

        public abstract Vector3Int GetWanderTarget(UnitEntity self, World world);
    }
}
