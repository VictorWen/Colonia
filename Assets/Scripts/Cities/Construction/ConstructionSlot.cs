using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cities.Construction.Projects;
using Items;

namespace Cities.Construction
{
    public class ConstructionSlot
    {
        private readonly City city;
        private CityConstruction cityConstruction;

        private IProject project;
        private string selectedProjectID;
        private Dictionary<string, int> allocatedResources;
        private float requiredConstructionProgress;
        private float constructionProgress;

        public ConstructionSlot(City city, CityConstruction cityConstruction)
        {
            this.city = city;
            this.cityConstruction = cityConstruction;
        }

        public void UpdateConstruction(GameMaster game)
        {
            if (selectedProjectID != null)
            {
                UpdateConstructionProgressCost(game);
                constructionProgress += cityConstruction.ConstructionDev;

                //FOR TESTING---------------------------------------
                //constructionProgress = requiredConstructionProgress;
                //--------------------------------------------------

                //Project is completed
                if (constructionProgress >= requiredConstructionProgress)
                {
                    project.Complete(city, game.World);
                    int pop = GlobalProjectDictionary.GetProjectData(selectedProjectID).Employment;
                    city.idlePop -= pop;
                    city.workingPop += pop;
                    CloseProject();
                }
            }
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

        public string GetProjectName()
        {
            if (selectedProjectID == null)
                return "EMPTY";
            return GlobalProjectDictionary.GetProjectData(selectedProjectID).Name;
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
