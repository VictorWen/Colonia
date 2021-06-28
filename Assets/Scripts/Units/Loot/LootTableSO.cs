using System.Collections.Generic;
using System;
using UnityEngine;
using Items;

namespace Units.Loot
{
    [CreateAssetMenu(fileName = "Loot Table", menuName = "Loot Table", order = 1)]
    public class LootTableSO : ScriptableObject
    {
        [Serializable]
        struct LootTableElement
        {
            public float chance;
            public string itemID;
        }

        [SerializeField] private List<LootTableElement> table;
        
        public LootTable ToLootTable()
        {
            List<float> chances = new List<float>();
            List<Item> items = new List<Item>();
            foreach (LootTableElement element in table) {
                chances.Add(element.chance);
                items.Add(new ResourceItem(element.itemID));
            }
            return new LootTable(chances, items);
        }
    }
}
