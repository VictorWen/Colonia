using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating whether to try use an ability, retreat, or wander.
    /// </summary>
    public abstract class NPCMainAI : MonoBehaviour
    {
        public abstract AIState GetState(NPCUnitEntity self, World world);

        public abstract Ability GetNextAbility(NPCUnitEntity self, World world);
    }
}
