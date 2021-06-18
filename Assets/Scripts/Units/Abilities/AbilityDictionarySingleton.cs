using System;
using System.Collections.Generic;


namespace Units.Abilities
{
    public class AbilityDictionarySingleton
    {
        private static AbilityDictionarySingleton singleInstance;

        private static IAbilityDictionary abilityDictionary;

        private AbilityDictionarySingleton()
        {
            // If/When needed, replace with another dictionary implementation
            abilityDictionary = new AbilitySODictionary();
        }

        public static AbilityDictionarySingleton Instance
        {
            get
            {
                if (singleInstance == null)
                    singleInstance = new AbilityDictionarySingleton();
                return singleInstance;
            }
        }

        public Ability GetAbility(string id)
        {
            return abilityDictionary.GetAbility(id);
        }
    }
}
