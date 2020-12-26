using Units;

namespace Items.UtilityItems.Potions
{
    public class Potion : UtilityItem
    {
        private readonly float initialValue;
        public override float Value
        {
            get
            {
                return initialValue * Uses / Hardness;
            }
        }

        private readonly PotionEffect[] effects;

        public Potion(string id, string name, int tier, int uses, float weight, float value, PotionEffect[] effects) : base(id, name, tier, uses, weight, "Potion")
        {
            this.initialValue = value;
            this.effects = effects;
        }

        protected override void OnUse(UnitEntityScript user)
        {
            foreach (PotionEffect effect in effects)
            {
                effect.Apply(user.Unit, user.Unit);
            }
        }
    }
}