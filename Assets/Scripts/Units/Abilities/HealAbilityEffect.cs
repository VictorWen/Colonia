using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Units.Abilities
{
    public class HealAbilityEffect : AbilityEffect
    {
        private readonly int healAmount;

        public HealAbilityEffect(int healAmount)
        {
            this.healAmount = healAmount;
        }

        public override void Apply(UnitEntity user, List<UnitEntity> targets)
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
