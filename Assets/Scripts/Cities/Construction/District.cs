using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public class District : ConstructedTileProject
    {
        private ResourceModifiers rmods;
        public List<Building> Buildings { get; private set; }
        private HashSet<string> invalidTiles;

        public override string Type { get { return "District"; } }

        public string Name { get; private set; }

        public int BuildingSlots { get; private set; }

        public District(string tier, int buildingSlots, string[] InvalidTiles, string name = null) : base (tier)
        {
            rmods = new ResourceModifiers();
            Buildings = new List<Building>();
            invalidTiles = new HashSet<string>();
            foreach (string s in InvalidTiles)
            {
                invalidTiles.Add(s);
            }

            Name = name;
            BuildingSlots = buildingSlots;
        }

        public void UpdatePendingResources(City city, GameMaster game)
        {

        }

        /*Returns a description of the district in relation to the building argument given.
          *Description includes:
          * Related buildings that are in the district
          * Related district modifiers
          * 
          */
        public string GetDistrictDescription()
        {
            return "TEST DISTRICT DESCRPTION";
        }

        public override IProject Copy()
        {
            return new District(ID, BuildingSlots, new string[0])
            {
                position = position,
                rmods = rmods,
                Buildings = Buildings,
                invalidTiles = invalidTiles
            };
        }

        public override string GetDescription()
        {
            return "District";
        }

        public override string GetSelectionInfo(GUIMaster gui)
        {
            return "Name: " + Name;
        }

        public override void Complete(City city, GUIMaster game)
        {
            base.Complete(city, game);
            Name = GenerateName();
            city.Districts.Add(this);
        }

        private string GenerateName()
        {
            return "TEST DISTRICT NAME";
        }

        public override bool IsValidTile(Vector3Int gridPos, WorldTerrain world, City city)
        {
            Vector3[] checks = new Vector3[] { new Vector3(-1, 0), new Vector3(-0.5f, 0.75f), new Vector3(0.5f, 0.75f), new Vector3(1, 0), new Vector3(0.5f, -0.75f), new Vector3(-0.5f, -0.75f) };
            bool validBorder = false;
            foreach (Vector3 check in checks)
            {
                Vector3Int checkPos = world.grid.WorldToCell(world.grid.CellToWorld(gridPos) + check);
                CityTile tile = (CityTile) world.cities.GetTile(checkPos);
                if (checkPos.Equals(city.Position) || (tile != null && tile.City == city && tile.Type.Equals("District")))
                {
                    validBorder = true;
                    break;
                }
            }

            return validBorder && !invalidTiles.Contains(world.terrain.GetTile(gridPos).name);
        }

        // Call when district is upgraded
        public void UpgradeFrom(District old)
        {
            rmods = old.rmods;
            Buildings = old.Buildings;
            Name = old.Name;
        }

        public override string GetTooltipText(Vector3Int position, WorldTerrain world)
        {
            return null;
        }
    }
}
