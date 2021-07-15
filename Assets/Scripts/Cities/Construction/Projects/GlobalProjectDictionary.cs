using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cities.Construction.Projects
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
                { "farm", new ProjectData("Farm", 20, 0, new string[] { }, new BasicTileImprovement("farm", "food", true, new string[] { "Grass" }, new Dictionary<string, int>() { { "wood", 10 } }))},
                { "lumber mill", new ProjectData("Lumber Mill", 20, 0, new string[] { }, new BasicTileImprovement("lumber mill", "wood", true, new string[] { "Forest" }, new Dictionary<string, int>(){ { "wood", 10 } }))},
                { "quarry", new ProjectData("Quarry", 30, 0, new string[] { "lumber mill" }, new BasicTileImprovement("quarry", "stone", false, new string[] {"Grass", "Hills", "Mountain" }, new Dictionary<string, int>(){ { "wood", 30 } }))},
                { "mine", new ProjectData("Mine", 40, 0, new string[] { "quarry" }, new ResourceTileImprovement("mine", new string[] { "steel", "unicornite" }, false, new Dictionary<string, int>(){ { "wood", 50 }, { "stone", 10 } }))},

                // Districts
                //{ "lower district", new ProjectData("Lower District", 0, 0, new string[] { }, new District("lower district", 4, new Dictionary<string, int>() { }, new string[] { }))},

                // Buildings Tier 0
                //{ "saw mill", new ProjectData("Saw Mill", 30, 100, new string[] { }, new Building("saw mill", new Dictionary<string, int>() { { "wood", 40 }, { "stone", 10 } })) },
                
                // TODO: REMOVE TEST BUILDING
                //{ "test", new ProjectData("TEST BUILDING", 0, 0, new string[] { }, new Building("test", new Dictionary<string, int>() { { "wood", 10 }, { "stone", 10 } })) },
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