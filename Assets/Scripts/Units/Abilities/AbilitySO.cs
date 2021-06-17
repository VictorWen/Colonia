using UnityEngine;
using System.Collections.Generic;

namespace Units.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability", order = 1)]
    public class AbilitySO : ScriptableObject
    {
        public string id;
        public string title;

        public int manaCost;

        public int range;
        public bool ignoreLineOfSight;

        public bool targetFriends = false;
        public bool targetEnemies = true;

        [SerializeReference] public List<AbilityEffect> effects = new List<AbilityEffect>();
    }
}
