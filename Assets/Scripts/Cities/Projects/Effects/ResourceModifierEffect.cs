using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Changes the effective cost of a resource
public class ResourceModifierEffect : BuildingEffect
{
    private readonly GlobalResourceDictionary.AttributeID attr;
    private readonly string id;
    private readonly float value;

    public ResourceModifierEffect(GlobalResourceDictionary.AttributeID attr, string id, float value)
    {
        this.id = id;
        this.attr = attr;
        this.value = value;
    }

    public override void Apply(City city)
    {
        city.ResourceMods.AddResourceMod(attr, id, value);
    }

    public override void Remove(City city)
    {
        city.ResourceMods.AddResourceMod(attr, id, -value);
    }

    public override string ToString()
    {
        string output = "";
        if (value > 0)
        {
            output += "Increases " + GlobalResourceDictionary.GetResourceData(id).name;
            output += " " + attr.ToString() + " by " + Mathf.RoundToInt(value * 100) + "%";
        }
        else if (value < 0)
        {
            output += "Decreases " + GlobalResourceDictionary.GetResourceData(id).name;
            output += " " + attr.ToString() + " by " + Mathf.RoundToInt(value * 100) + "%";
        }
        return output;
    }
}
