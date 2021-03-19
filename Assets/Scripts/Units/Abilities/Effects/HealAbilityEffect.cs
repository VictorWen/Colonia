using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiles;

namespace Units.Abilities
{
    public class HealAbilityEffect : AbilityEffect
    {
        private readonly int healAmount;

        public HealAbilityEffect(int healAmount)
        {
            this.healAmount = healAmount;
        }

        public override void Apply(BaseUnitEntity user, List<BaseUnitEntity> targets, IWorld world)
        {
            foreach (BaseUnitEntity target in targets)
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
