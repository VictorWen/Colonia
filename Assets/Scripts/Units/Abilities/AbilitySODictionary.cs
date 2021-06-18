using System.Collections.Generic;
using UnityEngine;

namespace Units.Abilities
{
    public class AbilitySODictionary : IAbilityDictionary
    {
        private static Dictionary<string, Ability> abilities;

        public AbilitySODictionary()
        {
            abilities = new Dictionary<string, Ability>();
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
