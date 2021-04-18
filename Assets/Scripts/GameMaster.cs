using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cities;
using Cities.Construction;
using Units;
using Units.Intelligence;
using Items;

//Handles background game state (Server)
public class GameMaster
{
    public World World { get; private set; }

    public Inventory GlobalInventory { get; private set; }

    private readonly Dictionary<string, float> pendingResources;
    private readonly List<City> cities;
    private readonly ResourceModifiers globalModifiers;

    public List<NPCIntelligence> npcList = new List<NPCIntelligence>();

    public GameMaster(World world)
    {
        this.World = world;
        pendingResources = new Dictionary<string, float>();
        GlobalInventory = new Inventory(-1);
        pendingResources = new Dictionary<string, float>();
        cities = new List<City>();
        globalModifiers = new ResourceModifiers();
    }

    private void SetupCapitalCity()
    {
        // Create Capital City
        // Fill it with resources
        // Place tile improvements <- this is difficult!
    }

    public void PlaceStarterTileImprovements(City city)
    {
        // Find candidate tiles for each tile improvement
        // Add tile improvement to city
        // Set World tile data to corresponding tile improvement <- how to identify what tile to put down?
        // Place tile improvement graphics down at corresponding tile <- is a higher level and should be somewhere else?
        Vector3Int farmTile = new Vector3Int(0, 0, 0);
        ProjectData farmProjectData = GlobalProjectDictionary.GetProjectData("farm");
        IProject farmProject = farmProjectData.Project;
        string farmPath = System.IO.Path.Combine("Projects", "Constructed Tiles", "farm");
        ConstructedTile farmConstructedTile = Resources.Load<ConstructedTile>(farmPath);
        World.PlaceConstructedTile(farmTile, farmConstructedTile);
        World.FinishConstructionOfCityTile(city, (ConstructedTileProject)farmProject, farmTile);
        city.AddNextTurnEffect((CityNextTurnEffect)farmProject);
    }

    // TODO: Move game control methods to a separate interface or class
    public void NextTurn(GUIMaster gui)
    {
        foreach (NPCIntelligence npc in npcList)
            npc.ExecuteCombat(this);

        World.ExecuteNPCTurns(this);
        
        foreach (City city in cities)
        {
            city.OnNextTurn(gui);
        }

        World.UnitsOnNextTurn(this);

        Dictionary<string, int> delta = new Dictionary<string, int>();
        foreach (KeyValuePair<string, float> resource in pendingResources)
        {
            int amount = (int) resource.Value;
            GlobalInventory.AddItem(new ResourceItem(resource.Key, amount));
            delta.Add(resource.Key, amount);
        }
        foreach(KeyValuePair<string, int> resource in delta)
        {
            pendingResources[resource.Key] -= resource.Value;
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

    public float GetResourceModifier(ModifierAttributeID attr, string id, City city = null, District district = null)
    {
        return globalModifiers.GetResourceMod(attr, id) * (city != null ? city.ResourceMods.GetResourceMod(attr, id) : 1) * (district != null ? district.ResourceMods.GetResourceMod(attr, id) : 1);
    }
}
