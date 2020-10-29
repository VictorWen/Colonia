using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public class Building : IProject
    {
        public string Type { get { return "Building"; } }
        public string ID { get; private set; }

        private District destination;

        public Building(string id)
        {
            ID = id;
        }

        public void Complete(City city, GUIMaster gui)
        {
            foreach (BuildingEffect effect in GlobalBuildingDictionary.GetCompletionEffects(ID))
            {
                effect.Apply(city);
            }
            destination.Buildings.Add(this);
        }

        public IProject Copy()
        {
            return new Building(ID);
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

        public string GetSelectionInfo(GUIMaster gui)
        {
            return "District: " + destination.Name + "\n";
        }

        public void OnDeselect(City city, GUIMaster gui)
        {
            //pass
        }

        public void OnSelect(City city, GUIMaster gui)
        {
            gui.districtSelectorScript.Enable(city, this, gui);
        }

        public void SetDistrict(District destination)
        {
            this.destination = destination;
        }
    }
}