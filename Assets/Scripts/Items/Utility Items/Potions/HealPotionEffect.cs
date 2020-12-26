

using Units;

namespace Items.UtilityItems.Potions
{
    class HealPotionEffect : PotionEffect
    {
        private readonly int healAmount;
        
        public HealPotionEffect(int healAmount)
        {
            this.healAmount = healAmount;
        }

        public override void Apply(UnitEntity user, UnitEntity target)
        {
            target.Heal(healAmount);
        }
    }
}
