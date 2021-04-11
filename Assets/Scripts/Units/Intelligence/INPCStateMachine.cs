using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// Determines what state the NPC should be in.
    /// Ex: COMBAT, WANDER, IDLE, etc.
    /// </summary>
    public interface INPCStateMachine
    {
        AIState GetState(UnitEntity self, World world);
    }
}
