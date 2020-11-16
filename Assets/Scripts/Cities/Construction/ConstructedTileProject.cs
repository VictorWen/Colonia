using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public abstract class ConstructedTileProject : IProject
    { 
        public string ID { get; protected set; }
        public abstract string ProjectType { get; }

        protected bool placed;
        protected Vector3Int position;
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
            this.position = position;
            this.upgradee = upgradee;
        }

        public bool IsConstructable(City city, GameMaster game)
        {
            // TODO: Move to a IsAfforable(position, city, game) method
            // Determine if an empty valid tile is affordable
            bool normalCost = true;
            foreach (KeyValuePair<string, int> resource in GetResourceCost(city, game))
            {
                if (game.GlobalInventory.GetResourceCount(resource.Key) < resource.Value)
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
                    if (game.World.cities.GetTile(position) != null && IsUpgradeableTile(position, game.World))
                    {
                        upgradee = ((ConstructedTile)game.World.cities.GetTile(position)).Project;
                        bool upgradeCost = true;
                        foreach (KeyValuePair<string, int> resource in GetResourceCost(city, game))
                        {
                            if (game.GlobalInventory.GetResourceCount(resource.Key) < resource.Value)
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

        public virtual void OnCancel(City city, GUIMaster gui)
        {
            gui.Game.World.cities.SetTile(position, null);
            //TODO: Manage deselect constructed tile upgrade case
        }
        
        public virtual void Complete(City city, GUIMaster gui)
        {
            ConstructedTile tile = (ConstructedTile) gui.Game.World.cities.GetTile(position);
            tile.FinishConstruction(city, ProjectType, this);
            gui.Game.World.cities.SetColor(position, new Color(1, 1, 1));

            if (upgradee != null)
            {
                //TODO: Manage replacing old constructed tile after upgrade
                OnUpgrade(upgradee);
            }
        }

        public abstract void OnUpgrade(ConstructedTileProject upgradee);

        public abstract bool IsValidTile(Vector3Int position, World world, City city);

        public abstract string GetTooltipText(Vector3Int position, World world);

        public abstract IProject Copy();

        public abstract string GetDescription();

        public abstract string GetSelectionInfo(GUIMaster gui);

        public abstract bool IsUpgradeableTile(Vector3Int position, World world);

    }
}
