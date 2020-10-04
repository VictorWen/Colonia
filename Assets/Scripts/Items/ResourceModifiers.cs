using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceModifiers
{
    private Dictionary<GlobalResourceDictionary.AttributeID, Dictionary<string, float>> mods;

    public ResourceModifiers()
    {
        mods = new Dictionary<GlobalResourceDictionary.AttributeID, Dictionary<string, float>>();
        foreach (GlobalResourceDictionary.AttributeID attr in Enum.GetValues(typeof(GlobalResourceDictionary.AttributeID)))
        {
            mods.Add(attr, new Dictionary<string, float>());
        }
    }

    public void AddResourceMod(GlobalResourceDictionary.AttributeID attr, string id, float value)
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

    public float GetResourceMod(GlobalResourceDictionary.AttributeID attr, string id)
    {
        return mods[attr][id];
    }
}