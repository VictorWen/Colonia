using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //TODO: decide whether to use ScriptableObjects or JSON/txt/xml files
            //Tile Improvement Tier 0
            { "farm", new ProjectData("farm", "Farm", new Dictionary<string, int>() { { "wood", 20 } }, 20, 0, new string[] { }, new TileImprovement("farm", "food", true, new string[] { "Grass" }))},
            { "lumber mill", new ProjectData("lumber mill", "Lumber Mill", new Dictionary<string, int>(){ { "wood", 10 } }, 20, 0, new string[] { }, new TileImprovement("lumber mill", "wood", true, new string[] { "Forest" }))},
            { "quarry", new ProjectData("quarry", "Quarry", new Dictionary<string, int>(){ { "wood", 30 } }, 30, 0, new string[] { }, new TileImprovement("quarry", "stone", false, new string[] {"Grass", "Hills", "Mountain" }))},
            
            //Buildings Tier 0
            { "saw mill", new ProjectData("saw mill", "Saw Mill", new Dictionary<string, int>() { { "wood", 40 }, { "stone", 10 } }, 30, 100, new string[] { }, new Building("saw mill", new BuildingEffect[] {
                new ResourceModifierEffect(GlobalResourceDictionary.AttributeID.Hardness, "wood", -0.1f)
            })) }
        };
    }

    // TODO: change to many wrappers? maybe
    public static ProjectData GetProjectData(string id)
    {
        return projects[id];
    }

    public static Dictionary<string, ProjectData> GetAllProjects()
    {
        return projects;
    }
}
