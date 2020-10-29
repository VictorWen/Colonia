using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cities;

//Handles background game state (Server)
public class GameMaster
{
    public WorldTerrain world { get; private set; }

    public Inventory GlobalInventory { get; private set; }

    private Dictionary<string, float> pendingResources;
    private List<City> cities;

    public GameMaster(WorldTerrain world)
    {
        this.world = world;
        pendingResources = new Dictionary<string, float>();
        GlobalInventory = new Inventory(-1);
        pendingResources = new Dictionary<string, float>();
        cities = new List<City>();
    }

    public void NextTurn(GUIMaster gui)
    {
        foreach (City city in cities)
        {
            city.OnNextTurn(gui);
        }
    }

    public void AddNewCity(City city)
    {
        cities.Add(city);
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
