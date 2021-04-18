using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Cities.Construction
{
    public class District : ConstructedTileProject
    {
        public ResourceModifiers ResourceMods { get; private set; }
        public List<Building> Buildings { get; private set; }
        private HashSet<string> invalidTiles;

        public override string ProjectType { get { return "District"; } }

        public string Name { get; private set; }

        public int BuildingSlots { get; private set; }

        public District(string tier, int buildingSlots, Dictionary<string, int> baseCost, string[] InvalidTiles, string name = null) : base (tier, baseCost)
        {
            ResourceMods = new ResourceModifiers();
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
            return new District(ID, BuildingSlots, baseResourceCost, new string[0])
            {
                position = position,
                ResourceMods = ResourceMods,
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

        public override bool IsValidTile(Vector3Int gridPos, World world, City city)
        {
            Vector3[] checks = new Vector3[] { new Vector3(-1, 0), new Vector3(-0.5f, 0.75f), new Vector3(0.5f, 0.75f), new Vector3(1, 0), new Vector3(0.5f, -0.75f), new Vector3(-0.5f, -0.75f) };
            bool validBorder = false;
            foreach (Vector3 check in checks)
            {
                Vector3Int checkPos = world.grid.WorldToCell(world.grid.CellToWorld(gridPos) + check);
                ConstructedTile tile = world.GetConstructedTile(checkPos);
                if (checkPos.Equals(city.Position) || (tile != null && tile.City == city && tile.Type.Equals("District")))
                {
                    validBorder = true;
                    break;
                }
            }

            return validBorder && !invalidTiles.Contains(world.GetTerrainTile(gridPos).name);
        }

        // Call when district is upgraded
        public override void OnUpgrade(ConstructedTileProject upgradee)
        {
            District old = (District)upgradee;
            ResourceMods = old.ResourceMods;
            Buildings = old.Buildings;
            Name = old.Name;
        }

        public override string GetTooltipText(Vector3Int position, World world)
        {
            return null;
        }

        public void OnNextTurn(City city, GameMaster game)
        {
            foreach (Building building in Buildings)
            {
                building.OnNextTurn(city, game);
            }
        }

        public override bool IsUpgradeableTile(Vector3Int position, World world)
        {
            if ((world.GetConstructedTile(position)).Completed)
            {
                ConstructedTileProject old = (world.GetConstructedTile(position)).Project;
                //TODO: move district levels to a higher level class
                string[] levels = { "lower district", "middle district", "upper district" };
                if (old.ProjectType == "District")
                {
                    int level = -1;
                    for (int i = 0; i < levels.Length - 1; i++)
                    {
                        if (ID == levels[i])
                        {
                            level = i;
                            break;
                        }
                    }
                    return levels[level + 1] == old.ID;
                }
            }
            return false;
        }
    }
}
