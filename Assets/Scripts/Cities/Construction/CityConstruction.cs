using System.Collections.Generic;
using System;
using Cities;
using Cities.Construction.Projects;
using Items;

namespace Cities.Construction
{
    // Tracks project completion progress and manages project event handling
    public class CityConstruction
    {
        public int ConstructionDev { get { return constructionDev; } }

        private readonly HashSet<string> completedProjects;
        private readonly int constructionDev;

        private readonly City city;

        private const int initialSlots = 1;
        public List<ConstructionSlot> Slots { get; private set; }

        public CityConstruction(City city)
        {
            this.city = city;
            completedProjects = new HashSet<string>();
            constructionDev = 4;//4;
            Slots = new List<ConstructionSlot>();
            
            for (int i = 0; i < initialSlots; i++)
                Slots.Add(new ConstructionSlot(city, this));
        }

        public void UpdateConstruction(GameMaster game)
        {
            foreach (ConstructionSlot slot in Slots)
                slot.UpdateConstruction(game);
        }

        public List<string> GetAvailableProjects()
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, ProjectData> pair in GlobalProjectDictionary.GetAllProjects())
            {
                bool available = pair.Value.WorkingPopPreReq <= city.workingPop && pair.Value.Employment <= city.idlePop;
                foreach (string req in pair.Value.ProjectPreReqs)
                {
                    if (!completedProjects.Contains(req))
                    {
                        available = false;
                        break;
                    }
                }

                if (available)
                    list.Add(pair.Key);
            }
            return list;
        }

        public void AddCompletedProject(string id)
        {
            completedProjects.Add(id);
        }
    }
}