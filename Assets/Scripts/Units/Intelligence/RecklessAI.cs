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
        public override AIState GetState(NPCUnitEntityAI self, World world)
        {
            // Check if there is any enemy visible
            foreach (Vector3Int visible in self.Visibles)
            {
                UnitEntity unitAt = world.UnitManager.GetUnitAt<UnitEntity>(visible);
                if (unitAt != null && self.Combat.IsEnemy(unitAt.Combat))
                {
                    return AIState.ABILITY;
                }
            }

            return AIState.WANDER;
        }

        public override Ability GetNextAbility(NPCUnitEntityAI self, World world)
        {
            return new Ability("attack", "Basic Attack", 0, 1, false, new AbilityEffect[] { new DamageAbilityEffect(0, true) }, new HexAbilityAOE(1));
        }
    }
}
