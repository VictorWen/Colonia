using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    public class Inventory
    {
        public float MaxWeight { get; private set; }

        public List<Item> Items { get; private set; }

        public Inventory(float maxWeight)
        {
            Items = new List<Item>();
            MaxWeight = maxWeight;
        }

        // Returns true if successful, false otherwise
        public bool AddItem(Item i)
        {
            if (MaxWeight == -1 || GetWeight() + i.Weight <= MaxWeight)
            {
                if (i.Type.Equals("Resource") && GetResourceCount(((ResourceItem)i).ID) > 0)
                {
                    foreach (Item item in Items)
                    {
                        if (item.Type.Equals("Resource") && ((ResourceItem)item).ID.Equals(i.ID))
                        {
                            item.Count += i.Count;
                            if (item.Count <= 0)
                            {
                                Items.Remove(item);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    Items.Add(i);
                }
                return true;
            }
            return false;
        }

        // Returns true if successful, false otherwise
        public bool MoveItem(int index, Inventory receiver)
        {
            if (receiver.AddItem(Items[index]))
            {
                Items.RemoveAt(index);
                return true;
            }
            return false;
        }

        public float GetWeight()
        {
            float sum = 0;
            foreach (Item i in Items)
            {
                sum += i.Weight;
            }
            return sum;
        }

        public int GetResourceCount(string id)
        {
            foreach (Item i in Items)
            {
                if (i.Type.Equals("Resource") && ((ResourceItem)i).ID.Equals(id))
                {
                    return i.Count;
                }
            }
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