using System;
using System.Collections.Generic;

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

        public override void Apply(UnitEntity caster, List<UnitEntity> targets)
        {
            foreach (UnitEntity target in targets)
            {
                float attack = attackMultiplier;
                if (isPhysicalDamage)
                    attack *= caster.Attack;
                else
                    attack *= caster.Magic;
                target.DealDamage(baseDamage + attack, caster, isPhysicalDamage);
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
