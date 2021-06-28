using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cities;
using Cities.Construction.Projects;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Constructed Tile", order = 1)]
public class ConstructedTile : Tile
{
    public City City { get; private set; }

    public string Type { get; private set; }

    public ConstructedTileProject Project { get; private set; }

    public bool Completed { get; private set; }

    public void StartConstruction()
    {
        Completed = false;
    }

    public void FinishConstruction(City city, string type, ConstructedTileProject project)
    {
        Debug.Log("City Tile Construction Finished: " + type);
        City = city;
        Type = type;
        Project = project;
        Completed = true;
    }
}
