using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ResourceModifiers
    {
        private readonly Dictionary<ModifierAttributeID, Dictionary<string, float>> mods;

        public ResourceModifiers()
        {
            mods = new Dictionary<ModifierAttributeID, Dictionary<string, float>>();
            foreach (ModifierAttributeID attr in Enum.GetValues(typeof(ModifierAttributeID)))
            {
                mods.Add(attr, new Dictionary<string, float>());
            }
        }

        public void AddResourceMod(ModifierAttributeID attr, string id, float value)
        {
            if (mods[attr].ContainsKey(id))
            {
                mods[attr][id] += value;
            }
            else
            {
                mods[attr].Add(id, 1 + value);
            }
        }

        public float GetResourceMod(ModifierAttributeID attr, string id)
        {
            if (mods[attr].ContainsKey(id))
                return mods[attr][id];
            else
                return 1;
        }
    }
}