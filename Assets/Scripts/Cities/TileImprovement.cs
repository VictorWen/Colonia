using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public class TileImprovement : ConstructedTileProject
    {
        private string resourceID;
        public bool UseFertility { get; private set; }
        //TODO: Formalize tile identification
        private HashSet<string> validTiles;

        public override string Type { get { return "Tile Improvement"; } }

        public TileImprovement(string id, string resource, bool fertility, string[] validTiles/*, Range from city?*/) : base(id)
        {
            resourceID = resource;
            UseFertility = fertility;
            this.validTiles = new HashSet<string>();
            foreach (string tileName in validTiles)
            {
                this.validTiles.Add(tileName);
            }
        }

        private TileImprovement(string id, string resource, bool fertility, HashSet<string> validTiles/*, Range from city?*/) : base (id)
        {
            ID = id;
            resourceID = resource;
            UseFertility = fertility;
            this.validTiles = validTiles;
        }

        public override void Complete(City city, GUIMaster gui)
        {
            base.Complete(city, gui);
            city.AddTileImprovement(this);
        }

        public override IProject Copy()
        {
            //same validtiles reference, but should be okay
            //TODO: check if validTile reference matters
            TileImprovement copy = new TileImprovement(ID, resourceID, UseFertility, validTiles)
            {
                position = position
            };
            return copy;
        }

        public void OnNextTurn(City city, GameMaster game)
        {
            float tilePower = UseFertility ? game.world.GetFertilityAtTile(position) : game.world.GetRichnessAtTile(position);
            float hardnessModifier = game.GetResourceModifier(ModifierAttributeID.HARDNESS, resourceID, city);
            float efficienyModifier = game.GetResourceModifier(ModifierAttributeID.EFFICIENCY, resourceID, city);
            city.AddResource(resourceID, (tilePower * efficienyModifier) / (GlobalResourceDictionary.GetResourceData(resourceID).hardness * hardnessModifier));
        }

        public override string GetDescription()
        {
            return "TILE IMPROVEMENT TEST DESCRIPTION";
        }

        public override bool IsValidTile(Vector3Int position, WorldTerrain world, City city)
        {
            return validTiles.Contains(world.terrain.GetTile(position).name);
        }

        public override string GetTooltipText(Vector3Int position, WorldTerrain world)
        {
            float aspect = UseFertility ? world.GetFertilityAtTile(position) : world.GetRichnessAtTile(position);
            return (UseFertility ? "Fertility: " : "Richness: ") + System.Math.Round(aspect, 2);
        }

        public override string GetSelectionInfo(GUIMaster gui)
        {
            float aspect = UseFertility ? gui.Game.world.GetFertilityAtTile(position) : gui.Game.world.GetRichnessAtTile(position);
            return (UseFertility ? "Fertility: " : "Richness: ") + System.Math.Round(aspect, 2);
        }
    }
}