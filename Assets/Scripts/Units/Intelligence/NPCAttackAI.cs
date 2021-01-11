using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating how to attack a target
    /// </summary>
    public interface NPCAttackAI
    {
        /// <summary>
        /// Called before DoAbilityAction. Determines what attack, ability, or item to use.
        /// </summary>
        /// <returns>If an ability is to be used, it's ID is returned. Otherwise, returns null</returns>
        string UpdateAI(UnitEntity self, UnitEntity target, World world);

        void CompleteAttackAction(UnitEntity self, World world);
    }
}
