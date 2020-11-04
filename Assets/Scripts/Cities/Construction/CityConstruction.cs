using System.Collections.Generic;
using Cities;

namespace Cities.Construction
{
    // Tracks project completion progress and manages project event handling
    public class CityConstruction
    {
        private List<string> completedProjects;
        private int constructionDev = 2;
        private IProject project;
        private string selectedProjectID;
        private Dictionary<string, int> allocatedResources;
        //TODO: Change to auto update property
        private float requiredConstructionProgress;
        private float constructionProgress;

        private City city;

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
                UpdateConstructionProgressCost();
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
                    SetProject(null, gui, false);
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
                foreach (KeyValuePair<string, int> resource in pair.Value.Costs)
                {
                    if (city.inv.GetResourceCount(resource.Key) < resource.Value)
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

        // TODO: formalize project setting
        // TODO: cancel project => updateGUI
        public void SetProject(string id, GUIMaster gui, bool deselect = true)
        {
            if (project != null && deselect)
            {
                project.OnDeselect(city, gui);
                foreach (KeyValuePair<string, int> resource in allocatedResources)
                {
                    city.inv.AddItem(new ResourceItem(resource.Key, resource.Value));
                }
            }
            project = null;

            selectedProjectID = id;
            allocatedResources = new Dictionary<string, int>();
            requiredConstructionProgress = 0;
            if (id != null)
            {
                ProjectData data = GlobalProjectDictionary.GetProjectData(id);
                project = data.Project;
                project.OnSelect(city, gui);
                UpdateConstructionProgressCost();

                //TODO: Incorporate modifiers
                //Assumes enough resources in inventory
                foreach (KeyValuePair<string, int> resource in data.Costs)
                {
                    city.inv.AddItem(new ResourceItem(resource.Key, -resource.Value));
                    allocatedResources.Add(resource.Key, resource.Value);
                }
            }
            constructionProgress = 0;
        }

        private void UpdateConstructionProgressCost()
        {
            requiredConstructionProgress = 0;
            foreach (KeyValuePair<string, int> cost in GlobalProjectDictionary.GetProjectData(selectedProjectID).Costs)
            {
                requiredConstructionProgress += GlobalResourceDictionary.GetResourceData(cost.Key).weight * cost.Value;
            }
        }

        //TODO: change to a property?
        public string GetSelectedProjectID()
        {
            return selectedProjectID;
        }

        public string GetDescription(GUIMaster gui)
        {
            string output = (selectedProjectID == null ? "No Selected Construction Project" : GlobalProjectDictionary.GetProjectData(selectedProjectID).ToString()) + "\n";
            if (project != null)
                output += "\n" + project.GetSelectionInfo(gui);
            output += "\n<b>Progress:</b> " + constructionProgress + "/" + requiredConstructionProgress;
            return output;
        }
    }
}