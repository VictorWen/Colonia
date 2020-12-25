using System;
using Units;

namespace Items.UtilityItems
{
    public abstract class UtilityItem : Item
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
        public abstract override float Value { get; }

        public int Uses { get; private set; }

        public UtilityItem(string id, string name, int tier, int uses, float weight, string type) : base(type)
        {
            this.id = id;
            this.name = name;
            this.tier = tier;
            hardness = uses;
            Uses = uses;
            this.weight = weight;
        }

        public void Use(UnitEntityScript user)
        {
            Uses--;
            OnUse(user);
            if (Uses == 0)
            {
                OnDestroy(user);
                Count = 0;
            }
        }

        protected abstract void OnUse(UnitEntityScript user);

        protected virtual void OnDestroy(UnitEntityScript user) { }

        public virtual void OnNextTurn() { }

    }
}