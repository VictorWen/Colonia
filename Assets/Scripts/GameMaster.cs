using Cities;
using Cities.Construction;
using Items;
using System;
using System.Collections.Generic;
using Units;
using Units.Combat;
using Units.Abilities;
using UnityEngine;

//Handles background game state (Server)
public class GameMaster
{
    public World World { get; private set; }

    public Inventory GlobalInventory { get; private set; }

    private readonly Dictionary<string, float> pendingResources;
    private readonly List<City> cities;
    private readonly ResourceModifiers globalModifiers;

    public List<NPCIntelligence> npcList = new List<NPCIntelligence>();
    public event Action<UnitEntity, string> OnUnitSpawn;

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
    }

    public void SpawnStarterHeroes()
    {
        SpawnUnitEntity("testPlayer", "The New Guy", new Vector3Int(2, 2, 0));
        //SpawnUnitEntity("testEnemy", "Test Enemy", new Vector3Int(-2, -2, 0));
    }

    public UnitEntity SpawnUnitEntity(string id, string name, Vector3Int position)
    {
        UnitEntityData unitData = GlobalUnitEntityDictionary.GetUnitEntityData("Unit Entities", id); // This might be too tightly coupled to Unity?
        UnitEntityCombatData combatData = UnitEntityCombatData.LoadFromSO(unitData);
        UnitEntity unitEntity = new UnitEntity(name, position, unitData.maxHealth, unitData.sight, unitData.isPlayerControlled, unitData.movementSpeed, World, combatData);
        OnUnitSpawn?.Invoke(unitEntity, id);
        return unitEntity;
    }

    public void PlaceStarterTileImprovements(City city)
    {
        HashSet<Vector3Int> tiles = city.GetCityRange(World);
        List<Vector3Int> sortedFertility = new List<Vector3Int>(tiles);
        sortedFertility.Sort((x1, x2) => CompareTileFertility(x1, x2));
        List<Vector3Int> sortedRichness = new List<Vector3Int>(tiles);
        sortedRichness.Sort((x1, x2) => CompareTileRichness(x1, x2));

        int farms = 3;
        int lumber = 2;
        int quarries = 1;

        PlaceNTileImprovements(farms, "farm", sortedFertility, city);
        PlaceNTileImprovements(lumber, "lumber mill", sortedFertility, city);
        PlaceNTileImprovements(quarries, "quarry", sortedRichness, city);
    }

    // TODO: Move game control methods to a separate interface or class
    public void NextTurn()
    {
        foreach (NPCIntelligence npc in npcList)
            npc.ExecuteCombat(this);

        World.ExecuteNPCTurns(this);
        
        foreach (City city in cities)
        {
            city.OnNextTurn(this);
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

    private int CompareTileFertility(Vector3Int tile1, Vector3Int tile2)
    {
        float fert1 = World.GetFertilityAtTile(tile1);
        float fert2 = World.GetFertilityAtTile(tile2);

        if (fert1 < fert2)
            return 1;
        else if (fert2 < fert1)
            return -1;
        else
            return 0;
    }

    private int CompareTileRichness(Vector3Int tile1, Vector3Int tile2)
    {
        float rich1 = World.GetRichnessAtTile(tile1);
        float rich2 = World.GetRichnessAtTile(tile2);

        if (rich1 < rich2)
            return 1;
        else if (rich2 < rich1)
            return -1;
        else
            return 0;
    }

    private void PlaceNTileImprovements(int n, string id, List<Vector3Int> tiles, City city)
    {
        for (int i = 0, count = 0; count < n && i < tiles.Count; i++)
        {
            if (PlaceCityTile(id, tiles[i], city))
                count++;
        }
    }

    private bool PlaceCityTile(string id, Vector3Int pos, City city)
    {
        ProjectData projectData = GlobalProjectDictionary.GetProjectData(id);
        ConstructedTileProject project = (ConstructedTileProject)projectData.Project;
        if (!project.IsValidTile(pos, World, city))
            return false;

        project.OnPlacement(pos);

        World.InstantiateConstructedTile(id, pos);
        project.Complete(city, World);

        return true;
    }

}
