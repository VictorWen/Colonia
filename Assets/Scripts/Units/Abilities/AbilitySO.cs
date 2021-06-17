using UnityEngine;
using System.Collections.Generic;

namespace Units.Abilities
{
    [CreateAssetMenu(fileName = "abilityID", menuName = "Ability", order = 1)]
    public class AbilitySO : ScriptableObject
    {
        [SerializeField] private string title;

        [SerializeField] private int manaCost;

        [SerializeField] private int range;
        [SerializeField] private bool ignoreLineOfSight;

        [SerializeField] private bool targetFriends = false;
        [SerializeField] private bool targetEnemies = true;

        [SerializeReference] private List<AbilityEffect> effects = new List<AbilityEffect>();
        [SerializeReference] private AbilityAOE aoe = new HexAbilityAOE();

        public List<AbilityEffect> Effects { get { return effects; } }
        public AbilityAOE AOE { get { return aoe; } set { aoe = value; } }

        public Ability GetAbility()
        {
            return new Ability(name, title, manaCost, range, ignoreLineOfSight, effects.ToArray(), aoe, targetFriends, targetEnemies);
        }
    }
}
