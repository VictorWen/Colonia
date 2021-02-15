using Units.Abilities;
using UnityEngine;

namespace Units.Intelligence
{
    /// <summary>
    /// Determines who or where to target.
    /// </summary>
    public abstract class NPCTargetingAI
    {
        public abstract Vector3Int GetAbilityTarget(NPCUnitEntityAI self, Ability ability, World world);

        public abstract Vector3Int GetManeuverTarget(NPCUnitEntityAI self, Ability ability, Vector3Int abilityTarget, World world);

        public abstract Vector3Int GetRetreatTarget(NPCUnitEntityAI self, World world);

        public abstract Vector3Int GetWanderTarget(NPCUnitEntityAI self, World world);
    }
}
