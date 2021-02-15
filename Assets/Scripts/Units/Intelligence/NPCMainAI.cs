using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating whether to try use an ability, retreat, or wander.
    /// </summary>
    public abstract class NPCMainAI
    {
        public abstract AIState GetState(NPCUnitEntityAI self, World world);

        public abstract Ability GetNextAbility(NPCUnitEntityAI self, World world);
    }
}
