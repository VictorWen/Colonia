using Units;

namespace Items.UtilityItems.Potions
{
    public abstract class PotionEffect
    {
        public abstract void Apply(UnitEntity user, UnitEntity target);
    }
}