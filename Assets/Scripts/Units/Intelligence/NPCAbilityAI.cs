using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// Called when the NPCMainAI.GetState returns AIState.ABILITY;
    /// Determines what ability to to use and who to target.
    /// </summary>
    public interface NPCAbilityAI
    {
        Ability GetNextAbility(NPCUnitEntity self, World world);

        UnitEntity GetAbilityTarget(UnitEntity self, Ability ability, World world);
    }
}
