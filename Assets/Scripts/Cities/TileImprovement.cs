using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileImprovement : IProject
{
    private string resourceID;
    public bool UseFertility { get; private set; }
    //TODO: Formalize tile identification
    private HashSet<string> validTiles;

    private TileImprovementGhostScript pointer;
    private Vector3Int position;

    public string Type { get { return "Tile Improvement"; } }
    public string ID { get; private set; }

    public TileImprovement(string id, string resource, bool fertility, string[] validTiles/*, Range from city?*/)
    {
        this.ID = id;
        resourceID = resource;
        UseFertility = fertility;
        this.validTiles = new HashSet<string>();
        foreach (string tileName in validTiles)
        {
            this.validTiles.Add(tileName);
        }
    }

    private TileImprovement(string id, string resource, bool fertility, HashSet<string> validTiles/*, Range from city?*/)
    {
        this.ID = id;
        resourceID = resource;
        UseFertility = fertility;
        this.validTiles = validTiles;
    }

    public void Complete(City city)
    {
        position = pointer.Position;
        Object.Destroy(pointer.gameObject);
        GUIMaster.main.world.cities.SetTile(position, Resources.Load<UnityEngine.Tilemaps.Tile>("Tiles" + System.IO.Path.DirectorySeparatorChar + ID));
        city.AddTileImprovement(this);
    }

    public void OnSelect(City city)
    {
        pointer = Object.Instantiate(GUIMaster.main.tileImprovementGhostScript);
        city.UpdateCityRange();
        pointer.PlaceTileImprovement(city, GUIMaster.main.world, this);
    }

    public void OnDeselect(City city)
    {
        Object.Destroy(pointer.gameObject);
    }

    public IProject Copy()
    {
        //same validtiles reference, but should be okay
        //TODO: check if validTile reference matters
        TileImprovement copy = new TileImprovement(ID, resourceID, UseFertility, validTiles)
        {
            pointer = pointer
        };
        return copy;
    }
    
    public void OnNextTurn(City city)
    {
        float tilePower = UseFertility ? GUIMaster.main.world.GetFertilityAtTile(position) : GUIMaster.main.world.GetRichnessAtTile(position);
        //TODO: add modifiers
        city.AddResource(resourceID, tilePower / GlobalResourceDictionary.GetResourceData(resourceID).hardness);
    }

    public string GetDescription()
    {
        return "TILE IMPROVEMENT TEST DESCRIPTION";
    }

    public bool IsValidTile(Vector3Int position)
    {
        return validTiles.Contains(GUIMaster.main.world.terrain.GetTile(position).name);
    }
}
