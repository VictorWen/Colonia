using System.Collections.Generic;
using UnityEngine;

namespace Units.Abilities
{
    public class AbilitySODictionarySingleton : IAbilityDictionary
    {
        private static AbilitySODictionarySingleton singleInstance;
        private static Dictionary<string, Ability> abilities;

        private AbilitySODictionarySingleton() { }

        public static AbilitySODictionarySingleton GetInstance()
        {
            if (singleInstance == null)
                singleInstance = new AbilitySODictionarySingleton();
            return singleInstance;
        }

        public Ability GetAbility(string id)
        {
            if (abilities.ContainsKey(id))
                return abilities[id];

            AbilitySO so = Resources.Load<AbilitySO>("Abilities/" + id);
            if (so == null)
                return null;
            Ability ability = so.GetAbility();
            abilities.Add(id, ability);
            return ability;
        }
    }
}
