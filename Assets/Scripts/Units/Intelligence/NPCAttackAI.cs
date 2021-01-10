using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating how to attack a target
    /// </summary>
    public interface NPCAttackAI
    {
        Ability GetAbilityTelegraph(UnitEntity self, UnitEntity target, World world);
    }
}
