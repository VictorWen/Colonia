using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Items.ItemActions;

namespace Items
{
    public abstract class Item
    {
        public string TypeName { get; private set; }
        public int Count { get; private set; }
        public abstract string Name { get; }
        public abstract int Tier { get; }
        public abstract float Hardness { get; }
        public abstract float Weight { get; }
        public abstract float Value { get; }
        public abstract string ID { get; }

        public abstract bool IsStackable { get; }

        protected Inventory inventory;

        //TODO: added Item sprite color

        public Item(string type, int count = 1)
        {
            TypeName = type;
            Count = count;
        }

        public void AttachInventoryListener(Inventory inventory)
        {
            if (inventory != null && !inventory.ContainsItem(this))
                return;
            this.inventory = inventory;
        }

        public override string ToString()
        {
            return GetAttributes() + "\n<b>" + TypeName + "</b>";
        }

        public virtual List<ItemAction> GetItemActions()
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

        public virtual void AddCount(int add)
        { 
            Count += add;
            if (Count < 0)
                Count = 0;
            UpdateInventory();
        }

        protected virtual void UpdateInventory()
        {
            if (inventory != null)
                inventory.UpdateItem(this);
        }
    }
}