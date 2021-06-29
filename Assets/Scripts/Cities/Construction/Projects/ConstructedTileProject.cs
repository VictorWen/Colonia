using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Cities.Construction.Projects
{
    public abstract class ConstructedTileProject : IProject
    {
        public string ID { get; protected set; }
        public abstract string ProjectType { get; }

        protected bool placed;
        public Vector3Int Position { get; protected set; }
        protected ConstructedTileProject upgradee;

        protected Dictionary<string, int> baseResourceCost;

        public ConstructedTileProject(string id, Dictionary<string, int> baseCost)
        {
            ID = id;
            baseResourceCost = baseCost;

            placed = false;
            upgradee = null;
        }

        public virtual Dictionary<string, int> GetResourceCost(City city, GameMaster game)
        {
            Dictionary<string, int> modifiedCosts = new Dictionary<string, int>();
            Dictionary<string, int> upgradeeCosts = null;
            if (upgradee != null)
                upgradeeCosts = upgradee.GetResourceCost(city, game);
            foreach (KeyValuePair<string, int> resource in baseResourceCost)
            {
                float cost = resource.Value;
                if (upgradee != null && upgradeeCosts.ContainsKey(resource.Key))
                    cost -= Mathf.Max(0, upgradeeCosts[resource.Key]);
                cost *= game.GetResourceModifier(ModifierAttributeID.CONSTRUCTION, resource.Key, city);
                modifiedCosts.Add(resource.Key, (int)cost);
            }
            return modifiedCosts;
        }

        public virtual void OnPlacement(Vector3Int position, ConstructedTileProject upgradee = null)
        {
            placed = true;
            this.Position = position;
            this.upgradee = upgradee;
        }

        public bool IsConstructable(City city, GameMaster game)
        {
            // TODO: Move to a IsAfforable(position, city, game) method
            // Determine if an empty valid tile is affordable
            bool normalCost = true;
            foreach (KeyValuePair<string, int> resource in GetResourceCost(city, game))
            {
                if (game.GlobalInventory.GetItemCount(resource.Key) < resource.Value)
                {
                    normalCost = false;
                    break;
                }
            }

            // Iterate through City Range and return if there is a valid and afforable tile
            foreach (Vector3Int position in city.GetCityRange(game.World))
            {
                if (IsValidTile(position, game.World, city))
                {
                    // If the valid tile is not empty, check if it can be upgraded and afforded
                    if (game.World.GetConstructedTile(position) != null && IsUpgradeableTile(position, game.World))
                    {
                        upgradee = game.World.GetConstructedTile(position).Project;
                        bool upgradeCost = true;
                        foreach (KeyValuePair<string, int> resource in GetResourceCost(city, game))
                        {
                            if (game.GlobalInventory.GetItemCount(resource.Key) < resource.Value)
                            {
                                upgradeCost = false;
                                break;
                            }
                        }
                        upgradee = null;

                        if (upgradeCost)
                        {
                            return true;
                        }
                    }

                    // If the valid tile is empty, check if the normal cost is affordable
                    else if (normalCost)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual IEnumerator OnSelect(City city, GUIMaster gui)
        {
            ConstructedTileGhost ghost = Object.Instantiate(gui.ghostPrefab);

            yield return ghost.Place(city, gui.Game.World, this, gui.GUIState);
            Object.Destroy(ghost.gameObject);

            yield break;
        }

        public bool IsSelected()
        {
            return placed;
        }

        public virtual void OnCancel(City city, World world)
        {
            world.PlaceConstructedTile(Position, null);
            //TODO: Manage deselect constructed tile upgrade case
        }

        public virtual void Complete(City city, World world)
        {
            world.FinishConstructionOfCityTile(city, this, Position, upgradee);
        }

        public abstract void OnUpgrade(ConstructedTileProject upgradee);

        public abstract bool IsValidTile(Vector3Int position, World world, City city);

        /* <summary>
         * Text used for hovering text above ConstructedTileGhost during project selection
         * </summary> */
        public abstract string GetTooltipText(Vector3Int position, World world);

        public abstract IProject Copy();

        public abstract string GetDescription();

        public abstract string GetSelectionInfo(World world);

        public abstract bool IsUpgradeableTile(Vector3Int position, World world);

    }
}
