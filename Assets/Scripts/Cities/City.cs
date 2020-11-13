using System.Collections.Generic;
using UnityEngine;
using Cities.Construction;

namespace Cities
{
    public class City
    {
        public string Name { get; private set; }
        public ResourceModifiers ResourceMods { get; private set; }

        // Construction fields
        public readonly CityConstruction construction;
        public List<string> AvailableProjects { get { return construction.GetAvailableProjects(); } }

        //TODO: TEMPORARY POPULATION IMPLEMENTATION, REMOVE
        public int population;
        public float popGrowthRate;
        public int idlePop;
        public int workingPop;

        private readonly List<TileImprovement> tileImprovements;

        // Districts
        //private int availableDistricts = 0; //TODO: implement available district
        public List<District> Districts { get; private set; }

        //TODO: Formalize City position
        public Vector3Int Position { get; private set; }
        private HashSet<Vector3Int> cityRange;

        public City(string name, Vector3Int position)
        {
            Name = name;
            this.Position = position;

            //TODO: TEMPORARY POPULATION IMPLEMENTATION, REMOVE
            population = 150;
            idlePop = population;
            workingPop = 0;
            popGrowthRate = 0.01f;

            ResourceMods = new ResourceModifiers();
            construction = new CityConstruction(this);
            tileImprovements = new List<TileImprovement>();
            Districts = new List<District>() { new District("city center", 5, new Dictionary<string, int>(), new string[0], "Test City Center")};
        }

        public void OnNextTurn(GUIMaster gui)
        {
            construction.UpdateConstruction(gui);
            foreach (District district in Districts)
            {
                district.OnNextTurn(this, gui.Game);
            }
            foreach (TileImprovement tileImprovement in tileImprovements)
            {
                tileImprovement.OnNextTurn(this, gui.Game);
            }

            //TODO: TEMPORARY POPULATION GROWTH IMPLEMENTATION, REMOVE
            int foodCost = Mathf.RoundToInt(population / 10f);
            if (gui.Game.GlobalInventory.GetResourceCount("food") >= foodCost)
            {
                gui.Game.GlobalInventory.AddItem(new ResourceItem("food", -foodCost));
                int delta = Mathf.RoundToInt(population * popGrowthRate);
                population += delta;
                idlePop += delta;
            }
            //--------------------------------------------------------
        }

        public void AddTileImprovement(TileImprovement ti)
        {
            tileImprovements.Add(ti);
        }

/*        public void AddResource(string id, float value)
        {
            //GameMaster.capital.GetInventory().AddItem(new ResourceItem(id, (int)(value / GlobalResourceDictionary.GetResourceData(id).hardness)));
            // TODO: add improper use case handle for AddResource
            inv.AddItem(new ResourceItem(id, Mathf.RoundToInt(value)));
        }*/

        public bool WithinCityRange(Vector3Int tilePos)
        {
            return cityRange.Contains(tilePos);
        }

        //TODO: Add CityRange visual
        public void UpdateCityRange(WorldTerrain world)
        {
            cityRange = new HashSet<Vector3Int>();
            //TODO: Formalize cityRadius
            int cityRadius = 5;

            Vector2[] checks = new Vector2[] { new Vector2(-1, 0), new Vector2(-0.5f, 0.75f), new Vector2(0.5f, 0.75f), new Vector2(1, 0), new Vector2(0.5f, -0.75f), new Vector2(-0.5f, -0.75f) };
            Grid grid = world.grid;

            List<Vector3> queue = new List<Vector3>();
            List<float> moves = new List<float>();
            Vector3 snap = grid.CellToWorld(grid.WorldToCell(Position));
            cityRange.Add(grid.WorldToCell(Position));
            queue.Add(snap);
            moves.Add(0);

            while (queue.Count > 0)
            {
                foreach (Vector2 v in checks)
                {
                    Vector3 tilePos = queue[0] + (Vector3)v;
                    float cost = world.IsReachable(cityRadius - moves[0], grid.WorldToCell(tilePos));
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
            cityRange.Remove(grid.WorldToCell(Position));
        }

        public HashSet<Vector3Int> GetCityRange(WorldTerrain world)
        {
            UpdateCityRange(world);
            return cityRange;
        }

        public string GetDescription(GameMaster game)
        {
            string output = "<b>City Info</b>\n";
            //TOOD: formalize population
            output += "Population: " + population + " +" + Mathf.RoundToInt(popGrowthRate * 100) + "%/turn\n";
            output += "Idle Population: " + idlePop + "\n";
            output += "Working Population: " + workingPop + "\n";
            output += "Wealth: WIP" + "\n";
            output += "<b>Construction Resources</b>\n";
            output += "Wood: " + game.GlobalInventory.GetResourceCount("wood") + "\n";
            output += "Stone: " + game.GlobalInventory.GetResourceCount("stone") + "\n";
            return output;
        }
    }
}