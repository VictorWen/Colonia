using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Cities.Construction
{
    public class ResourceTileImprovement : ConstructedTileProject, CityNextTurnEffect
    {
        public override string ProjectType => "Resource Improvement";

        private readonly HashSet<string> resources;
        private readonly HashSet<string> validUpgrades;
        private readonly bool useFertility;

        public ResourceTileImprovement(string id, string[] resources, bool useFertility, Dictionary<string, int> baseCost, string[] validUpgrades = null) : base(id, baseCost)
        {
            this.resources = new HashSet<string>();
            if (resources != null)
            {
                foreach (string resource in resources)
                {
                    this.resources.Add(resource);
                }
            }
            this.useFertility = useFertility;
            this.validUpgrades = new HashSet<string>();
            if (validUpgrades != null)
            {
                foreach (string upgrade in validUpgrades)
                {
                    this.validUpgrades.Add(upgrade);
                }
            }
        }

        public override void Complete(City city, GUIMaster gui)
        {
            base.Complete(city, gui);
            city.AddNextTurnEffect(this);
        }

        public void OnNextTurn(City city, GameMaster game)
        {
            string id = game.World.ResourceMap.ResourceLocations[position];
            
            float tilePower = useFertility ? game.World.GetFertilityAtTile(position) : game.World.GetRichnessAtTile(position);
            float hardnessModifier = game.GetResourceModifier(ModifierAttributeID.HARDNESS, id, city);
            float efficienyModifier = game.GetResourceModifier(ModifierAttributeID.EFFICIENCY, id, city);
            game.AddPendingResource(id, tilePower * efficienyModifier / (GlobalResourceDictionary.GetResourceData(id).hardness * hardnessModifier));
        }

        private ResourceTileImprovement(ResourceTileImprovement copy) : base(copy.ID, copy.baseResourceCost)
        {
            this.resources = copy.resources;
            this.useFertility = copy.useFertility;
            this.validUpgrades = copy.validUpgrades;
        }

        public override void OnUpgrade(ConstructedTileProject upgradee)
        {
            throw new System.NotImplementedException();
        }

        public override IProject Copy()
        {
            return new ResourceTileImprovement(this);
        }

        public override string GetDescription()
        {
            return "TEST RESOURCES TILE IMPROVEMENT DESCRIPTION";
        }

        public override string GetSelectionInfo(GUIMaster gui)
        {
            float aspect = useFertility ? gui.Game.World.GetFertilityAtTile(position) : gui.Game.World.GetRichnessAtTile(position);
            return (useFertility ? "Fertility: " : "Richness: ") + System.Math.Round(aspect, 2);
        }

        public override string GetTooltipText(Vector3Int position, World world)
        {
            float aspect = useFertility ? world.GetFertilityAtTile(position) : world.GetRichnessAtTile(position);
            return (useFertility ? "Fertility: " : "Richness: ") + System.Math.Round(aspect, 2);
        }

        public override bool IsUpgradeableTile(Vector3Int position, World world)
        {
            return (world.GetConstructedTile(position)).Completed && validUpgrades.Contains((world.GetConstructedTile(position)).Project.ID);
        }

        public override bool IsValidTile(Vector3Int position, World world, City city)
        {
            return world.ResourceMap.ResourceLocations.ContainsKey(position) && resources.Contains(world.ResourceMap.ResourceLocations[position]);
        }
    }
}
