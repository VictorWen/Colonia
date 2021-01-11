using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating whether to try use an ability, retreat, or wander
    /// </summary>
    public interface NPCMainAI
    {
        AIState GetState(NPCUnitEntity self, World world);
    }
}
