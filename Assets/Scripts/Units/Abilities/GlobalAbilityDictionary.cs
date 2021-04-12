using System;
using System.Collections.Generic;


namespace Units.Abilities
{
    public static class GlobalAbilityDictionary
    {
        private readonly static Dictionary<string, Ability> abilities;

        static GlobalAbilityDictionary()
        {
            abilities = new Dictionary<string, Ability>();

            // Basic Attack
            AbilityEffect[] attackEffects = new AbilityEffect[] { new DamageAbilityEffect(0, true, 1) };
            AddAbility("attack", "Basic Attack", 0, 1, false, attackEffects, new HexAbilityAOE(0));

            // Fireball
            AbilityEffect[] fireballEffects = new AbilityEffect[] { new DamageAbilityEffect(5, false, 0.8f) };
            AddAbility("fireball", "Fireball", 3, 3, false, fireballEffects, new HexAbilityAOE(1));
        }

        private static void AddAbility(string id, string name, int mana, int range, bool ignoreLineOfSight, AbilityEffect[] effects, AbilityAOE areaOfEffect)
        {
            abilities.Add(id, new Ability(id, name, mana, range, ignoreLineOfSight, effects, areaOfEffect));
        }

        public static Ability GetAbility(string id)
        {
            if (abilities.ContainsKey(id))
                return abilities[id];
            return null;
        }
    }
}
