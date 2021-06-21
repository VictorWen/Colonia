using Items.ItemActions;
using System;
using System.Collections.Generic;
using Units;
using Units.Abilities;

namespace Items.UtilityItems
{
    public class UtilityItem : Item
    {
        public readonly string id;
        public override string ID { get { return id; } }

        private readonly string name;
        public override string Name { get { return name; } }

        private readonly int tier;
        public override int Tier { get { return tier; } }

        private readonly float hardness;
        public override float Hardness { get { return hardness; } } // Number of Uses

        private readonly float weight;
        public override float Weight { get { return weight; } }

        public override bool IsStackable { get { return false; } }

        private readonly float initialValue;
        public override float Value
        {
            get
            {
                return initialValue * Uses / Hardness;
            }
        }

        public int Uses { get; private set; }

        public UtilityItemAbility Ability { get; private set; }

        public UtilityItem(string id, string name, int tier, int uses, float weight, string type, float value, 
            int range, AbilityEffect[] effects, AbilityAOE aoe, bool targetFriends = true, bool targetEnemies = false) : base(type)
        {
            this.id = id;
            this.name = name;
            this.tier = tier;
            hardness = uses;
            Uses = uses;
            this.weight = weight;
            this.initialValue = value;
            Ability = new UtilityItemAbility(this, id, name, range, false, effects, aoe, targetFriends, targetEnemies);
        }

        public void Use()
        {
            Uses--;
            if (Uses <= 0)
            {
                Count--;
                if (Count > 0)
                    Uses = (int) hardness;
            }
        }

        public override List<ItemAction> GetItemActions()
        {
            List<ItemAction> actions = base.GetItemActions();
            actions.Add(new UseItemAction(this));
            return actions;
        }

        public virtual bool IsUsable()
        {
            return Uses > 0 && Count > 0;
        }

        public virtual void OnNextTurn() { }
    }
}