using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Units.Abilities
{
    public class HealAbilityEffect : AbilityEffect
    {
        public override string EffectTypeName { get { return "Heal Effect"; } }
        [SerializeField] private int healAmount;

        public HealAbilityEffect() { }

        public HealAbilityEffect(int healAmount)
        {
            this.healAmount = healAmount;
        }

        public override void Apply(UnitEntity user, List<UnitEntity> targets, IWorld world)
        {
            foreach (UnitEntity target in targets)
            {
                target.Heal(healAmount);
            }
        }

        public override string GetDescription()
        {
            return "Heals target for " + healAmount + " health";
        }
    }
}
