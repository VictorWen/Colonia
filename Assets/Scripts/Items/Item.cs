using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Items.ItemActions;

namespace Items
{
    public abstract class Item
    {
        public string Type { get; private set; }
        public int Count { get; set; }
        public abstract string Name { get; }
        public abstract int Tier { get; }
        public abstract float Hardness { get; }
        public abstract float Weight { get; }
        public abstract float Value { get; }
        public abstract string ID { get; }

        //TODO: added Item sprite color

        public Item(string type, int count = 1)
        {
            Type = type;
            Count = count;
        }

        public override string ToString()
        {
            return GetAttributes() + "\n<b>" + Type + "</b>";
        }

        public virtual List<ItemAction> GetActions()
        {
            return new List<ItemAction>();
        }

        protected virtual string GetAttributes()
        {
            string text = Name + " (" + Count + ")\n";
            text += "Tier: " + Tier + "\n";
            text += "Hardness: " + Hardness + "\n";
            text += "Weight: " + Weight + "\n";
            text += "Value: " + Value + "\n";
            return text;
        }
    }
}