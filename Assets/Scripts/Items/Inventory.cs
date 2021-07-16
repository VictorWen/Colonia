using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    public class Inventory
    {
        private readonly float maxWeight;
        private float currentWeight;

        private readonly Dictionary<string, Item> stackableItems;
        public List<Item> Items { get; private set; }

        public Inventory(float maxWeight)
        {
            stackableItems = new Dictionary<string, Item>();
            Items = new List<Item>();
            this.maxWeight = maxWeight;
            currentWeight = 0;
        }

        // Returns true if successful, false otherwise
        public bool AddItem(Item item)
        {
            if (maxWeight == -1 || currentWeight + item.Weight <= maxWeight)
            {
                if (item.IsStackable)
                    AddStackableItemToCollections(item);
                else
                    Items.Add(item);

                item.AttachInventoryListener(this);
                currentWeight += item.Weight;
                return true;
            }
            return false;
        }

        private void AddStackableItemToCollections(Item item)
        {
            if (!stackableItems.ContainsKey(item.ID))
            {
                if (item.Count <= 0)
                    return;
                stackableItems.Add(item.ID, item);
                Items.Add(item);
            }
            else
                stackableItems[item.ID].AddCount(item.Count);
        }

        public void RemoveZeroCountItems()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Count <= 0)
                    RemoveItem(i);
            }
        }

        public void UpdateItem(Item item)
        {
            if (item.Count <= 0)
                RemoveItem(item);
        }

        public void RemoveItem(int index)
        {
            Item item = Items[index];
            RemoveItem(item);
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
            if (item.IsStackable)
                stackableItems.Remove(item.ID);
            item.AttachInventoryListener(null);
            currentWeight -= item.Weight;
        }

        public void AddItemCount(string id, int count)
        {
            if (stackableItems.ContainsKey(id))
            {
                Item item = stackableItems[id];
                currentWeight -= item.Weight;
                item.AddCount(count);
                currentWeight += item.Weight;
            }
        }

        public bool ContainsItem(Item item)
        {
            return Items.Contains(item);
        }

        public int GetItemCount(string id)
        {
            if (stackableItems.ContainsKey(id))
                return stackableItems[id].Count;
            return 0;
        }

        public void Sort(ItemSortID sort)
        {
            switch (sort)
            {
                case ItemSortID.NAME:
                    Items.Sort((Item x, Item y) => x.Name.CompareTo(y.Name));
                    break;
            }
        }

        public enum ItemSortID
        {
            NAME
        }
    }
}