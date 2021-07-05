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

        private readonly List<string> completedProjects;
        private readonly int constructionDev;
        private IProject project;
        private string selectedProjectID;
        private Dictionary<string, int> allocatedResources;
        private float requiredConstructionProgress;
        private float constructionProgress;

        private readonly City city;

        public List<ConstructionSlot> Slots { get; private set; }

        public CityConstruction(City city)
        {
            this.city = city;
            completedProjects = new List<string>();
            constructionDev = 20;//4;
            selectedProjectID = null;
            Slots = new List<ConstructionSlot>();
            Slots.Add(new ConstructionSlot(city, this));
        }

        public void UpdateConstruction(GameMaster game)
        {
            foreach (ConstructionSlot slot in Slots)
            {
                slot.UpdateConstruction(game);
            }
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

        public void SetProject(IProject selectedProject, GameMaster game)
        {
            if (project != null)
                CancelProject(game);
            CloseProject();

            // Update values
            selectedProjectID = selectedProject.ID;
            project = selectedProject;
            UpdateConstructionProgressCost(game);

            // Take needed resources
            // Assumes enough resources in inventory
            foreach (KeyValuePair<string, int> resource in selectedProject.GetResourceCost(city, game))
            {
                game.GlobalInventory.AddItem(new ResourceItem(resource.Key, -resource.Value));
                allocatedResources.Add(resource.Key, resource.Value);
            }
        }

        private void CloseProject()
        {
            allocatedResources = new Dictionary<string, int>();
            requiredConstructionProgress = 0;
            constructionProgress = 0;
            selectedProjectID = null;
            project = null;
        }

        public void CancelProject(GameMaster game)
        {
            project.OnCancel(city, game.World);
            foreach (KeyValuePair<string, int> resource in allocatedResources)
                game.GlobalInventory.AddItem(new ResourceItem(resource.Key, resource.Value));
        }

        private void UpdateConstructionProgressCost(GameMaster game)
        {
            requiredConstructionProgress = 0;
            foreach (KeyValuePair<string, int> cost in project.GetResourceCost(city, game))
                requiredConstructionProgress += GlobalResourceDictionary.GetResourceData(cost.Key).weight * cost.Value;
        }

        public string GetDescription(GameMaster game)
        {
            string output = (selectedProjectID == null ? 
                "No Selected Construction Project" : 
                GlobalProjectDictionary.GetProjectData(selectedProjectID).GetDescription(city, game)) + "\n";
            if (project != null)
                output += "\n" + project.GetSelectionInfo(game.World);
            output += "\n<b>Progress:</b> " + constructionProgress + "/" + requiredConstructionProgress;
            return output;
        }
    }
}