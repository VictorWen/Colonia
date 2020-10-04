using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles background game state (Server)
public class GameMaster
{
    public Inventory GlobalInventory { get; private set; }

    private Dictionary<string, float> pendingResources;

    public GameMaster()
    {
        pendingResources = new Dictionary<string, float>();
        GlobalInventory = new Inventory(-1);
    }
    
    public void AddPendingResource(string id, float value)
    {
        if (pendingResources.ContainsKey(id))
        {
            pendingResources[id] += value;
        }
        else
        {
            pendingResources.Add(id, value);
        }
    }
}
