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

    public List<UnitEntity> Units { get { return unitManager.Units; } }
    private readonly UnitEntityManager unitManager;

    public GameMaster(World world)
    {
        this.World = world;
        pendingResources = new Dictionary<string, float>();
        GlobalInventory = new Inventory(-1);
        pendingResources = new Dictionary<string, float>();
        cities = new List<City>();
        globalModifiers = new ResourceModifiers();
        unitManager = World.UnitManager;
    }

    // TODO: Move game control methods to a separate interface or class
    public void NextTurn(GUIMaster gui)
    {
        foreach (City city in cities)
        {
            city.OnNextTurn(gui);
        }

        foreach (UnitEntity unit in unitManager.Units)
        {
            unit.OnNextTurn(this);
        }

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

    public UnitEntity AddNewTestUnit(string name, bool playerControlled, Vector3Int position, UnitEntityScript script)
    {
        UnitEntity unit = new UnitEntity(name, playerControlled, position, unitManager, script);
        unitManager.AddUnit(unit);
        return unit;
    }

    public NPCUnitEntity AddNewTestNPCUnit(string name, Vector3Int position, NPCTargetingAI targetAI, NPCMovementAI moveAI, NPCAttackAI attackAI, UnitEntityScript script)
    {
        NPCUnitEntity unit = new NPCUnitEntity(name, position, targetAI, moveAI, attackAI, unitManager, script);
        unitManager.AddUnit(unit);
        return unit;
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
