using System.Collections;
using System.Collections.Generic;
using Items;

namespace Cities.Construction
{
    public class Building : IProject
    {
        public string ProjectType { get { return "Building"; } }
        public string ID { get; private set; }

        private District destination;
        private int destIndex;
        private bool finishedSelection;

        private readonly Dictionary<string, int> baseCost;
        private readonly HashSet<string> upgradeables;

        public Building(string id, Dictionary<string, int> baseCost, string[] upgradeables = null)
        {
            ID = id;
            this.baseCost = baseCost;
            this.upgradeables = new HashSet<string>();
            if (upgradeables != null)
            {
                foreach (string s in upgradeables)
                {
                    this.upgradeables.Add(s);
                }
            }

            finishedSelection = false;
        }

        private Building(Building copy)
        {
            ID = copy.ID;
            baseCost = copy.baseCost;
            upgradeables = copy.upgradeables;

            destination = null;
            destIndex = -1;
            finishedSelection = false;
        }

        public IProject Copy()
        {
            return new Building(this);
        }

        public Dictionary<string, int> GetResourceCost(City city, GameMaster game)
        {
            Dictionary<string, int> modifiedCosts = new Dictionary<string, int>();
            foreach(KeyValuePair<string, int> resource in baseCost)
            {
                float cost = resource.Value * game.GetResourceModifier(ModifierAttributeID.CONSTRUCTION, resource.Key, city);
                if (destIndex != -1)
                    cost *= 0.75f;
                modifiedCosts.Add(resource.Key, (int) cost);
            }
            return modifiedCosts;
        }

        public bool IsConstructable(City city, GameMaster game)
        {
            // Check if at least one district has an available slot
            // OR if there is a slot that can be upgraded
            foreach (District district in city.Districts)
            {
                if (district.Buildings.Count < district.BuildingSlots)
                    return true;
                foreach (Building building in district.Buildings)
                {
                    if (upgradeables.Contains(building.ID))
                        return true;
                }
            }

            return false;
        }

        public IEnumerator OnSelect(City city, GUIMaster gui)
        {
            gui.districtSelectorScript.Enable(city, this, gui);
            while (!finishedSelection)
            {
                yield return null;
            }
        }

        public void FinishSelection(District destination, int index = -1)
        {
            finishedSelection = true;
            this.destination = destination;
            this.destIndex = index;
        }

        public bool IsSelected()
        {
            return destination != null;
        }

        public void OnCancel(City city, World world)
        {
            destination = null;
        }

        public void Complete(City city, World world)
        {
            foreach (BuildingEffect effect in GlobalBuildingDictionary.GetCompletionEffects(ID))
            {
                effect.Apply(city);
            }

            if (destIndex == -1)
            {
                destination.Buildings.Add(this);
            }
            else
            {
                destination.Buildings[destIndex] = this;
            } 
        }

        public void OnNextTurn(City city, GameMaster game)
        {
            foreach (BuildingEffect effect in GlobalBuildingDictionary.GetNextTurnEffects(ID))
            {
                effect.Apply(city);
            }
        }

        public string GetSelectionInfo(World world)
        {
            return "District: " + (destination != null ? destination.Name : "No District Selected");
        }

        public string GetDescription()
        {
            string descr = "BUILDING TEST DESCRIPTION\n";

            descr += "On Completion: \n";
            foreach (BuildingEffect effect in GlobalBuildingDictionary.GetCompletionEffects(ID))
            {
                descr += "\t" + effect.ToString() + "\n";
            }
            descr += "On Next Turn:";
            foreach (BuildingEffect effect in GlobalBuildingDictionary.GetNextTurnEffects(ID))
            {
                descr += "\n\t" + effect.ToString();
            }
            return descr;
        }
    }
}