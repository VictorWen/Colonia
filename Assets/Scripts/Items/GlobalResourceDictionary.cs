using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public static class GlobalResourceDictionary
    {
        private static Dictionary<string, ResourceData> resources;

        static GlobalResourceDictionary()
        {
            resources = new Dictionary<string, ResourceData>();

            AddResource("stone", "Stone", 0, 2f, 1.5f, 1.5f, 1f);
            AddResource("wood", "Wood", 0, 1f, 1f, 1f, 1f);
            AddResource("food", "Food", 0, 0.5f, 1f, 0.5f, 0.5f);
            AddResource("steel", "Steel Ingot", 1, 2f, 2f, 2f, 2f);
            //AddResources("iron", "Iron", 1, 2f, 2f, 2f, 2f);
        }

        private static void AddResource(string id, string name, int tier, float hardness, float shape, float weight, float value)
        {
            resources.Add(id, new ResourceData(name, tier, hardness, shape, weight, value));
        }

        public static ResourceData GetResourceData(string id)
        {
            return resources[id];
        }

        public struct ResourceData
        {
            public readonly string name;
            public readonly int tier;
            public readonly float hardness;
            public readonly float shape;
            public readonly float weight;
            public readonly float value;

            public ResourceData(string name, int tier, float hardness, float shape, float weight, float value)
            {
                this.name = name;
                this.tier = tier;
                this.hardness = hardness;
                this.shape = shape;
                this.weight = weight;
                this.value = value;
            }
        }
    }
}