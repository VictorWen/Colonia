using System.Collections.Generic;
using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// AI that always attacks any Unit within sight, never retreats.
    /// Only wanders when there is nothing to attack.
    /// </summary>
    public class RecklessAI : NPCMainAI
    {
        public AIState GetState(NPCUnitEntity self, World world)
        {
            // Check if there is any enemy visible
            foreach (Vector3Int visible in self.VisibleTiles)
            {
                if (world.UnitManager.Positions.ContainsKey(visible) && self.IsEnemy(world.UnitManager.Positions[visible]))
                {
                    return AIState.ABILITY;
                }
            }

            return AIState.WANDER;
        }

        public Ability GetNextAbility(NPCUnitEntity self, World world)
        {
            return new Ability("attack", "Basic Attack", 0, 1, false, new AbilityEffect[] { new DamageAbilityEffect(0, true) }, new HexAbilityAOE(1));
        }
    }
}
