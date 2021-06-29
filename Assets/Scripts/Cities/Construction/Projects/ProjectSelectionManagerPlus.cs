using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cities.Construction.Projects
{
    public interface IProjecteSelectionManager
    {
        void SelectedConstructedTile(City city, IProject constructedTileProject);

        void SelectBuilding(City city, IProject buildingProject);
    }

    public class ProjectSelectionManagerPlus
    {
        private readonly GUIMaster gui;

        public ProjectSelectionManagerPlus(GUIMaster gui)
        {
            this.gui = gui;
        }

        public void SelectConstructedTile(City city, IProject constructedTileProject)
        {
            ConstructedTileProject project = (ConstructedTileProject)constructedTileProject;
            ConstructedTileGhost ghost = UnityEngine.Object.Instantiate(gui.ghostPrefab);
            ghost.Place2(city, gui.Game.World, project, gui.GUIState);
        }

        public void SelectBuilding(City city, IProject buildingProject)
        {
            Building project = (Building)buildingProject;
            gui.districtSelectorScript.Enable(city, project, gui);
        }
    }
}
