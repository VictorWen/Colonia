using System;
using System.Collections.Generic;
using Tiles;

namespace Units.Abilities
{
    public class DamageAbilityEffect : AbilityEffect
    {
        private readonly int baseDamage;
        private readonly bool isPhysicalDamage;
        private readonly float attackMultiplier;

        public DamageAbilityEffect(int baseDamage, bool isPhysicalDamage, float attackMultiplier = 1f)
        {
            this.baseDamage = baseDamage;
            this.isPhysicalDamage = isPhysicalDamage;
            this.attackMultiplier = attackMultiplier;
        }

        public override void Apply(BaseUnitEntity caster, List<BaseUnitEntity> targets, IWorld world)
        {
            foreach (BaseUnitEntity target in targets)
            {
                float attack = attackMultiplier;
                if (isPhysicalDamage)
                    attack *= caster.Combat.Attack;
                else
                    attack *= caster.Combat.Magic;
                target.Combat.DealDamage(baseDamage + attack, caster.Combat, isPhysicalDamage);
            }
        }

        public override string GetDescription()
        {
            if (isPhysicalDamage)
            {
                return "Deals " + baseDamage + " + " + Math.Round(attackMultiplier, 2) + "*Attack Physical Damage"; 
            }
            else
            {
                return "Deals " + baseDamage + " + " + Math.Round(attackMultiplier, 2) + "*Magic Magical Damage";
            }
        }
    }
}
