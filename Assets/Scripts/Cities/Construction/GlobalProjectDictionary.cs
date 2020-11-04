using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cities.Construction
{
    /*
     * Used to store and create all projects available for the capital.
     */
    public static class GlobalProjectDictionary
    {
        private static readonly Dictionary<string, ProjectData> projects;

        static GlobalProjectDictionary()
        {
            projects = new Dictionary<string, ProjectData>
            {
                //TODO: decide whether to use code or JSON files
                //Tile Improvement Tier 0
                { "farm", new ProjectData("farm", "Farm", new Dictionary<string, int>() { { "wood", 20 } }, 20, 0, new string[] { }, new TileImprovement("farm", "food", true, new string[] { "Grass" }))},
                { "lumber mill", new ProjectData("lumber mill", "Lumber Mill", new Dictionary<string, int>(){ { "wood", 10 } }, 20, 0, new string[] { }, new TileImprovement("lumber mill", "wood", true, new string[] { "Forest" }))},
                { "quarry", new ProjectData("quarry", "Quarry", new Dictionary<string, int>(){ { "wood", 30 } }, 30, 0, new string[] { }, new TileImprovement("quarry", "stone", false, new string[] {"Grass", "Hills", "Mountain" }))},

                // Districts
                { "lower district", new ProjectData("lower district", "Lower District", new Dictionary<string, int>() { }, 0, 0, new string[] { }, new District("lower district", 4, new string[] { }))},

                // Buildings Tier 0
                { "saw mill", new ProjectData("saw mill", "Saw Mill", new Dictionary<string, int>() { { "wood", 40 }, { "stone", 10 } }, 30, 100, new string[] { }, new Building("saw mill")) },
                
                // TODO: REMOVE TEST BUILDING
                { "test", new ProjectData("test", "TEST BUILDING", new Dictionary<string, int>() { { "wood", 10 }, { "stone", 10 } }, 0, 0, new string[] { }, new Building("test")) },
            };
        }

        public static ProjectData GetProjectData(string id)
        {
            return projects[id];
        }

        public static Dictionary<string, ProjectData> GetAllProjects()
        {
            return projects;
        }
    }
}