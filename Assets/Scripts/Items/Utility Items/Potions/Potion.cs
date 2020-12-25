using Units;

namespace Items.UtilityItems.Potions
{
    public class Potion : UtilityItem
    {
        public override float Value => throw new System.NotImplementedException();

        private readonly PotionEffect[] effects;

        public Potion(string id, string name, int tier, int uses, float weight, PotionEffect[] effects) : base(id, name, tier, uses, weight, "Potion")
        {
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