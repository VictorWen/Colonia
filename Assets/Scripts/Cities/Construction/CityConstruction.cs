using System.Collections.Generic;
using Cities;

namespace Cities.Construction
{
    // Tracks project completion progress and manages project event handling
    public class CityConstruction
    {
        private readonly List<string> completedProjects;
        private readonly int constructionDev = 2;
        private IProject project;
        private string selectedProjectID;
        private Dictionary<string, int> allocatedResources;
        //TODO: Change to auto update property
        private float requiredConstructionProgress;
        private float constructionProgress;

        private readonly City city;

        public CityConstruction(City city)
        {
            this.city = city;
            completedProjects = new List<string>();
            constructionDev = 20;//4;
            selectedProjectID = null;
        }

        // TODO: formalize construction tick
        public void UpdateConstruction(GUIMaster gui)
        {
            if (selectedProjectID != null)
            {
                UpdateConstructionProgressCost(gui.Game);
                constructionProgress += constructionDev;

                //FOR TESTING---------------------------------------
                //constructionProgress = requiredConstructionProgress;
                //--------------------------------------------------

                //Project is completed
                if (constructionProgress >= requiredConstructionProgress)
                {
                    project.Complete(city, gui);
                    int pop = GlobalProjectDictionary.GetProjectData(selectedProjectID).Employment;
                    city.idlePop -= pop;
                    city.workingPop += pop;
                    CloseProject(gui);
                }
            }
        }

        public List<string> GetAvailableProjects()
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, ProjectData> pair in GlobalProjectDictionary.GetAllProjects())
            {
                //TODO: Fix selected project availability question
                //if (pair.Key != selectedProjectID)
                //{
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
                {
                    list.Add(pair.Key);
                }
                //}
            }
            return list;
        }

        public void SetProject(string id, IProject selectedProject, GUIMaster gui)
        {
            CloseProject(gui);

            // Reset counters;
            allocatedResources = new Dictionary<string, int>();
            requiredConstructionProgress = 0;
            constructionProgress = 0;

            // Update values
            selectedProjectID = id;
            project = selectedProject;
            UpdateConstructionProgressCost(gui.Game);

            // Take needed resources
            //TODO: Incorporate modifiers
            //Assumes enough resources in inventory
            foreach (KeyValuePair<string, int> resource in selectedProject.GetResourceCost(city, gui.Game))
            {
                gui.Game.GlobalInventory.AddItem(new ResourceItem(resource.Key, -resource.Value));
                allocatedResources.Add(resource.Key, resource.Value);
            }
        }

        public void CloseProject(GUIMaster gui)
        {
            if (project != null)
            {
                project.OnCancel(city, gui);
                foreach (KeyValuePair<string, int> resource in allocatedResources)
                {
                    gui.Game.GlobalInventory.AddItem(new ResourceItem(resource.Key, resource.Value));
                }
            }
            //TODO: SetProject to the None Project on CloseProject
        }

        private void UpdateConstructionProgressCost(GameMaster game)
        {
            requiredConstructionProgress = 0;
            foreach (KeyValuePair<string, int> cost in project.GetResourceCost(city, game))
            {
                requiredConstructionProgress += GlobalResourceDictionary.GetResourceData(cost.Key).weight * cost.Value;
            }
        }

        public string GetDescription(GUIMaster gui)
        {
            string output = (selectedProjectID == null ? "No Selected Construction Project" : GlobalProjectDictionary.GetProjectData(selectedProjectID).GetDescription(city, gui.Game)) + "\n";
            if (project != null)
                output += "\n" + project.GetSelectionInfo(gui);
            output += "\n<b>Progress:</b> " + constructionProgress + "/" + requiredConstructionProgress;
            return output;
        }
    }
}