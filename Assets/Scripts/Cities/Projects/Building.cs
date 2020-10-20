using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : IProject
{
    public string Type { get { return "Building"; } }
    public string ID { get; private set; }

    private List<BuildingEffect> completionEffects;
    private List<BuildingEffect> nextTurnEffects;

    public Building(string id, BuildingEffect[] effects)
    {
        ID = id;
        completionEffects = new List<BuildingEffect>();
        nextTurnEffects = new List<BuildingEffect>();
        foreach (BuildingEffect effect in effects)
        {
            if (effect.OnComplete) 
            {
                completionEffects.Add(effect);
            }
            else 
            { 
                nextTurnEffects.Add(effect);
            }
        }
    }

    private Building(string id, List<BuildingEffect> completionEffects, List<BuildingEffect> nextTurnEffects)
    {
        this.ID = id;
        this.completionEffects = completionEffects;
        this.nextTurnEffects = nextTurnEffects;
    }

    public void Complete(City city, GUIMaster gui)
    {
        foreach (BuildingEffect effect in completionEffects){
            effect.Apply(city);
        }
    }

    public IProject Copy()
    {
        return new Building(ID, completionEffects, nextTurnEffects);
    }

    public string GetDescription()
    {
        string descr = "BUILDING TEST DESCRIPTION\n";
        descr += "On Completion: \n";
        foreach (BuildingEffect effect in completionEffects)
        {
            descr += "\t" + effect.ToString() + "\n";
        }
        descr += "On Next Turn:";
        foreach (BuildingEffect effect in nextTurnEffects)
        {
            descr += "\n\t" + effect.ToString();
        }
        return descr;
    }

    public void OnDeselect(City city, GUIMaster gui)
    {
        //pass
    }

    public void OnSelect(City city, GUIMaster gui)
    {
        //pass
        //TODO: District selection?
    }
}
