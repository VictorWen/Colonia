using System;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Units.Abilities
{
    public class DamageAbilityEffect : AbilityEffect
    {
        public override string EffectTypeName { get { return "Damage Effect"; } }

        [SerializeField] private int baseDamage;
        [SerializeField] private bool isPhysicalDamage = true;
        [SerializeField] private float attackMultiplier = 1;

        public DamageAbilityEffect() { }

        public DamageAbilityEffect(int baseDamage, bool isPhysicalDamage, float attackMultiplier = 1f)
        {
            this.baseDamage = baseDamage;
            this.isPhysicalDamage = isPhysicalDamage;
            this.attackMultiplier = attackMultiplier;
        }

        public override void Apply(UnitEntity caster, List<UnitEntity> targets, IWorld world)
        {
            foreach (UnitEntity target in targets)
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
