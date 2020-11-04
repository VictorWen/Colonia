using UnityEngine;
using UnityEngine.UI;

namespace Cities.Construction
{
    public class ProjectButton : MonoBehaviour
    {
        public Button button;
        public Text text;

        public ConstructionPanelScript ProjectSelector { get; set; }
        public string ProjectID { get; set; }

        public void OnClick()
        {
            ProjectSelector.SelectProject(this);
        }
    }
}