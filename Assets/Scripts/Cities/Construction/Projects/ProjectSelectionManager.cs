using System.Collections;
using UnityEngine;

namespace Cities.Construction.Projects
{
    public interface IProjectVisitor
    {
        void VisitConstructionTile(City city, IProject constructedTileProject);

        void VisitBuilding(City city, IProject buildingProject);
    }

    public interface ProjectSelectionController
    {
        IEnumerator StartSelection();
    }

    public class ProjectSelectionManager : MonoBehaviour, IProjectVisitor
    {
        private World world;
        private GUIStateManager guiState;
        private ConstructedTileGhost ghostPrefab;
        private DistrictSelectorScript districtSelectorScript;
        private CityGUIPanelScript cityGUI;
        private ConstructionPanelScript construction;

        public GUIMaster gui;

        private void Start()
        {
            world = gui.Game.World;
            guiState = gui.GUIState;
            ghostPrefab = gui.ghostPrefab;
            districtSelectorScript = gui.districtSelectorScript;
            cityGUI = gui.cityGUI;
        }

        public void SetConstructionPanel(ConstructionPanelScript panel)
        {
            this.construction = panel;
        }

        public void VisitConstructionTile(City city, IProject constructedTileProject)
        {
            ConstructedTileProject project = (ConstructedTileProject)constructedTileProject;
            ConstructedTileGhost ghost = Instantiate(ghostPrefab);
            ghost.Enable(city, world, project, guiState);
            StartSelection(ghost, project);
        }

        public void VisitBuilding(City city, IProject buildingProject)
        {
            Building project = (Building)buildingProject;
            districtSelectorScript.Enable(city, project, cityGUI);
            StartSelection(districtSelectorScript, project);
        }

        private void StartSelection(ProjectSelectionController controller, IProject project)
        {
            StartCoroutine(SelectionCoroutine(controller, project));
        }

        private IEnumerator SelectionCoroutine(ProjectSelectionController controller, IProject project)
        {
            yield return StartCoroutine(controller.StartSelection());
            FinishSelection(project);
            yield break;
        }

        private void FinishSelection(IProject filledForm)
        {
            construction.FinishSelection(filledForm);
        }
    }
}
