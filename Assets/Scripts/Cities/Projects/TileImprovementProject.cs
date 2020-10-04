/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileImprovementProject : ProjectData
{
    private readonly string resourceID;
    private readonly bool useFertility;

    public TileImprovementProject(string id, string name, Dictionary<string, int> costs, int workPopPreReq, string[] projPreReq, string resourceID, bool useFertility) : base(id, name, costs, workPopPreReq, projPreReq, "Tile Improvement")
    {
        this.resourceID = resourceID;
        this.useFertility = useFertility;
    }

    public override void OnCompletion(City city)
    { 
        city.construction.AddTileImprovement(resourceID, useFertility);  
    }

    public override void OnSelect(City city, ConstructionGUIScript gui)
    {
        city.CloseCityGUI();
        TileImprovementGhostScript ghost = gui.CreateTileImprovementGhost(city, ID);
        city.construction.SetProject(ID, ghost.gameObject);
        city.OpenCityGUI();
    }
}
*/