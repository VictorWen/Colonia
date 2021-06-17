using UnityEngine;

namespace Units.Abilities
{
    public class AbilitySODictionarySingleton : IAbilityDictionary
    {
        private static AbilitySODictionarySingleton singleInstance;

        private AbilitySODictionarySingleton() { }

        public static AbilitySODictionarySingleton GetInstance()
        {
            if (singleInstance == null)
                singleInstance = new AbilitySODictionarySingleton();
            return singleInstance;
        }

        public Ability GetAbility(string id)
        {
            return null;
        }
    }
}
