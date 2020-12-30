using Units;
using Units.Abilities;

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

        private readonly Ability potionAbility;

        public Potion(string id, string name, int tier, int uses, float weight, float value, AbilityEffect[] effects) : base(id, name, tier, uses, weight, "Potion")
        {
            this.initialValue = value;
            potionAbility = new Ability(id, name, 0, 1, true, effects, new HexAbilityAOE(0), true);
        }

        protected override void OnUse(UnitEntityScript user)
        {
            user.SelectAbilityTarget(potionAbility);
        }
    }
}