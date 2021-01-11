using Units;
using Units.Abilities;
using UnityEngine;

namespace Items.UtilityItems
{
    public class UtilityItemAbility : Ability
    {
        private readonly UtilityItem item;

        public UtilityItemAbility(UtilityItem item, string id, string name, int range, bool ignoreLineOfSight, 
            AbilityEffect[] effects, AbilityAOE areaOfEffect, bool targetFriends = true, bool targetEnemies = false) 
            : base(id, name, 0, range, ignoreLineOfSight, effects, areaOfEffect, targetFriends, targetEnemies)
        {
            this.item = item;
        }

        public override void Cast(UnitEntity caster, Vector3Int target, World world)
        {
            base.Cast(caster, target, world);
            item.Use();
        }
    }
}
