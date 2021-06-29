using System.Collections.Generic;
using UnityEngine;
using Cities.Construction;
using Items;
using Tiles;
using Cities.Construction.Projects;

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

        private readonly List<CityNextTurnEffect> nextTurnEffects;

        // Districts
        //private int availableDistricts = 0; //TODO: implement available district
        public List<District> Districts { get; private set; }

        //TODO: Formalize City position
        public Vector3Int Position { get; private set; }
        private HashSet<Vector3Int> cityRange;

        private const int CITY_RADIUS = 4;

        public City(string name, Vector3Int position, IWorld world)
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
            nextTurnEffects = new List<CityNextTurnEffect>();
            Districts = new List<District>() { new District("city center", 5, new Dictionary<string, int>(), new string[0], "Test City Center")};

            UpdateCityRange(world);
            cityRange.Add(position);
            world.UpdatePlayerVision(new HashSet<Vector3Int>(), cityRange);
            cityRange.Remove(position);
        }

        public void OnNextTurn(GameMaster game)
        {
            construction.UpdateConstruction(game);
            foreach (District district in Districts)
            {
                district.OnNextTurn(this, game);
            }
            foreach (CityNextTurnEffect nextTurnEffect in nextTurnEffects)
            {
                nextTurnEffect.OnNextTurn(this, game);
            }

            //TODO: TEMPORARY POPULATION GROWTH IMPLEMENTATION, REMOVE
            int foodCost = Mathf.RoundToInt(population / 10f);
            if (game.GlobalInventory.GetItemCount("food") >= foodCost)
            {
                game.GlobalInventory.AddItem(new ResourceItem("food", -foodCost));
                int delta = Mathf.RoundToInt(population * popGrowthRate);
                population += delta;
                idlePop += delta;
            }
            //--------------------------------------------------------
        }

        public void AddNextTurnEffect(CityNextTurnEffect effect)
        {
            nextTurnEffects.Add(effect);
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
        public void UpdateCityRange(IWorld world)
        {
            cityRange = new HashSet<Vector3Int>();
            cityRange = world.GetTilesInRange(Position, CITY_RADIUS);
            cityRange.Remove(Position);
        }

        public HashSet<Vector3Int> GetCityRange(World world)
        {
            //UpdateCityRange(world);
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
            output += "Wood: " + game.GlobalInventory.GetItemCount("wood") + "\n";
            output += "Stone: " + game.GlobalInventory.GetItemCount("stone") + "\n";
            return output;
        }
    }
}