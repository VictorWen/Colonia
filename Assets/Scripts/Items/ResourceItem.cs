using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    public class ResourceItem : Item
    {
        public override string Name { get { return GlobalResourceDictionary.GetResourceData(id).name; } }
        public override int Tier { get { return GlobalResourceDictionary.GetResourceData(id).tier; } }
        public override float Hardness { get { return GlobalResourceDictionary.GetResourceData(id).hardness; } }
        public float Shape { get { return GlobalResourceDictionary.GetResourceData(id).shape; } }
        public override float Weight { get { return GlobalResourceDictionary.GetResourceData(id).weight; } }
        public override float Value { get { return GlobalResourceDictionary.GetResourceData(id).value; } }

        private readonly string id;
        public override string ID { get { return id; } }

        public ResourceItem(string id, int count = 1) : base("Resource", count)
        {
            this.id = id;
        }
    }
}