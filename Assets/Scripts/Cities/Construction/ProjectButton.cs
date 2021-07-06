using UnityEngine;
using UnityEngine.UI;
using Scripts;
using Cities.Construction.Projects;

namespace Cities.Construction
{
    public class ProjectButton : MonoBehaviour, ITooltippable
    {
        public Button button;
        public Text text;
        public TooltipOnHoverScript tooltipScript;

        public ConstructionPanelScript ProjectSelector { get; set; }
        public string ProjectID { get; set; }

        private City city;
        private GameMaster game;

        public void Initialize(City city, GameMaster game, ConstructionPanelScript projectSelector, string projectID, RectTransform tooltipObject)
        {
            this.city = city;
            this.game = game;
            this.ProjectSelector = projectSelector;
            this.ProjectID = projectID;
            text.text = GlobalProjectDictionary.GetProjectData(projectID).Name;
            this.tooltipScript.Initialize(this, tooltipObject, tooltipObject.GetComponentInChildren<Text>(), 15, true, false);
        }

        public string GetTooltipText()
        {
            return GlobalProjectDictionary.GetProjectData(ProjectID).GetDescription(city, game);
        }

        public void OnClick()
        {
            ProjectSelector.SelectProject(this);
        }
    }
}