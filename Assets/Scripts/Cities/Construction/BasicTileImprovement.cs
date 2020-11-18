using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public class BasicTileImprovement : ConstructedTileProject, CityNextTurnEffect
    {
        private readonly string resourceID;
        public bool UseFertility { get; private set; }
        //TODO: Formalize tile identification
        private readonly HashSet<string> validTiles;
        private readonly HashSet<string> validUpgrades;

        public override string ProjectType { get { return "Tile Improvement"; } }

        // validUpgrades should be a string array of TileImprovement IDs that can be replaced with this TileImprovement
        public BasicTileImprovement(string id, string resource, bool fertility, string[] validTiles, Dictionary<string, int> baseCost, string[] validUpgrades = null/*, Range from city?*/) : base(id, baseCost)
        {
            resourceID = resource;
            UseFertility = fertility;
            this.validTiles = new HashSet<string>();
            foreach (string tileName in validTiles)
            {
                this.validTiles.Add(tileName);
            }
            this.validUpgrades = new HashSet<string>();
            if (validUpgrades != null)
            {
                foreach (string tileID in validUpgrades)
                {
                    this.validUpgrades.Add(tileID);
                }
            }
        }

        private BasicTileImprovement(BasicTileImprovement copy) : base (copy.ID, copy.baseResourceCost)
        {
            resourceID = copy.resourceID;
            UseFertility = copy.UseFertility;
            validTiles = copy.validTiles;
            validUpgrades = copy.validUpgrades;
        }

        public override void Complete(City city, GUIMaster gui)
        {
            base.Complete(city, gui);
            city.AddNextTurnEffect(this);
        }

        public override void OnUpgrade(ConstructedTileProject upgradee)
        {
            //City.RemoveTileImprovement((Tile Improvement) upgradee);
        }

        public override IProject Copy()
        {
            //same validtiles reference, but should be okay
            //TODO: check if validTile reference matters
            BasicTileImprovement copy = new BasicTileImprovement(this)
            {
                position = position
            };
            return copy;
        }

        public void OnNextTurn(City city, GameMaster game)
        {
            float tilePower = UseFertility ? game.World.GetFertilityAtTile(position) : game.World.GetRichnessAtTile(position);
            float hardnessModifier = game.GetResourceModifier(ModifierAttributeID.HARDNESS, resourceID, city);
            float efficienyModifier = game.GetResourceModifier(ModifierAttributeID.EFFICIENCY, resourceID, city);
            game.AddPendingResource(resourceID, tilePower * efficienyModifier / (GlobalResourceDictionary.GetResourceData(resourceID).hardness * hardnessModifier));
        }

        public override string GetDescription()
        {
            return "TILE IMPROVEMENT TEST DESCRIPTION";
        }

        public override bool IsValidTile(Vector3Int position, World world, City city)
        {
            return validTiles.Contains(world.terrain.GetTile(position).name);
        }

        public override string GetTooltipText(Vector3Int position, World world)
        {
            float aspect = UseFertility ? world.GetFertilityAtTile(position) : world.GetRichnessAtTile(position);
            return (UseFertility ? "Fertility: " : "Richness: ") + System.Math.Round(aspect, 2);
        }

        public override string GetSelectionInfo(GUIMaster gui)
        {
            float aspect = UseFertility ? gui.Game.World.GetFertilityAtTile(position) : gui.Game.World.GetRichnessAtTile(position);
            return (UseFertility ? "Fertility: " : "Richness: ") + System.Math.Round(aspect, 2);
        }

        public override bool IsUpgradeableTile(Vector3Int position, World world)
        {
            return ((ConstructedTile)world.cities.GetTile(position)).Completed && validUpgrades.Contains(((ConstructedTile) world.cities.GetTile(position)).Project.ID);
        }
    }
}