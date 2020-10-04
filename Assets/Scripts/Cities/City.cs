using System.Collections.Generic;
using UnityEngine;

public class City
{
    public string Name { get; private set; }
    public ResourceModifiers ResourceMods { get; private set; }

    // Construction fields
    // TODO: formalize construction
    public readonly CityConstruction construction;
    public List<string> AvailableProjects { get { return construction.GetAvailableProjects(); } }

    //TODO: Temporary! Formalize city inventory
    public Inventory inv;

    //TODO: TEMPORARY POPULATION IMPLEMENTATION, REMOVE
    public int population;
    public float popGrowthRate;
    public int idlePop;
    public int workingPop;

    private List<TileImprovement> tileImprovements;
    private List<Building> buildings;

    //TODO: Formalize City position
    private Vector3Int position;
    private HashSet<Vector3Int> cityRange;

    public City(string name, Vector3Int position)
    {
        Name = name;
        this.position = position;

        //TODO: TEMPORARY POPULATION IMPLEMENTATION, REMOVE
        population = 150;
        idlePop = population;
        workingPop = 0;
        popGrowthRate = 0.01f;
        
        ResourceMods = new ResourceModifiers();
        construction = new CityConstruction(this);
        tileImprovements = new List<TileImprovement>();
    }

    public void OnNextTurn()
    {
        construction.UpdateConstruction();
        foreach (TileImprovement ti in tileImprovements)
        {
            ti.OnNextTurn(this);
        }

        //TODO: TEMPORARY POPULATION GROWTH IMPLEMENTATION, REMOVE
        int foodCost = Mathf.RoundToInt(population / 10f);
        if (inv.GetResourceCount("food") >= foodCost)
        {
            inv.AddItem(new ResourceItem("food", -foodCost));
            int delta = Mathf.RoundToInt(population * popGrowthRate);
            population += delta;
            idlePop += delta;
        }
        //--------------------------------------------------------
    }

    public float GetResourceAttribute(string id, GlobalResourceDictionary.AttributeID attr)
    {
        //TODO: Move to ResourceModifiers?
        return Mathf.Max(GlobalResourceDictionary.GetResourceAttribute(id, attr) * ResourceMods.GetResourceMod(attr, id), 0.1f);
    }

    public void AddTileImprovement(TileImprovement ti)
    {
        tileImprovements.Add(ti);
    }

    public void AddResource(string id, float value)
    {
        //GameMaster.capital.GetInventory().AddItem(new ResourceItem(id, (int)(value / GlobalResourceDictionary.GetResourceData(id).hardness)));
        inv.AddItem(new ResourceItem(id, Mathf.RoundToInt(value)));
    }

    public bool WithinCityRange(Vector3Int tilePos)
    {
        return cityRange.Contains(tilePos);
    }

    //TODO: Add CityRange visual
    public void UpdateCityRange()
    {
        cityRange = new HashSet<Vector3Int>();
        //TODO: Formalize cityRadius
        int cityRadius = 5;
        
        Vector2[] checks = new Vector2[] { new Vector2(-1, 0), new Vector2(-0.5f, 0.75f), new Vector2(0.5f, 0.75f), new Vector2(1, 0), new Vector2(0.5f, -0.75f), new Vector2(-0.5f, -0.75f) };
        Grid grid = GUIMaster.main.world.grid;

        List<Vector3> queue = new List<Vector3>();
        List<float> moves = new List<float>();
        Vector3 snap = grid.CellToWorld(grid.WorldToCell(position));
        cityRange.Add(grid.WorldToCell(position));
        queue.Add(snap);
        moves.Add(0);

        while (queue.Count > 0)
        {
            foreach (Vector2 v in checks)
            {
                Vector3 tilePos = queue[0] + (Vector3)v;
                float cost = GUIMaster.main.world.IsReachable(cityRadius - moves[0], grid.WorldToCell(tilePos));
                Vector3Int gridTilePos = grid.WorldToCell(tilePos);
                if (!cityRange.Contains(gridTilePos) && cost >= 0)
                {
                    //movement.SetTile(gridTilePos, cyan);
                    cityRange.Add(gridTilePos);

                    queue.Add(tilePos);
                    moves.Add(cost + moves[0]);
                }
            }
            queue.RemoveAt(0);
            moves.RemoveAt(0);
        }
        cityRange.Remove(grid.WorldToCell(position));
    }

    public override string ToString()
    {
        string output = "<b>City Info</b>\n";
        //TOOD: formalize population
        output += "Population: " + population + " +" + Mathf.RoundToInt(popGrowthRate * 100) + "%/turn\n";
        output += "Idle Population: " + idlePop + "\n";
        output += "Working Population: " + workingPop + "\n";
        output += "Wealth: WIP" + "\n";
        output += "<b>Construction Resources</b>\n";
        output += "Wood: " + inv.GetResourceCount("wood") + "\n";
        output += "Stone: " + inv.GetResourceCount("stone") + "\n";
        return output;
    }
}

