using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cities;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/City Tile", order = 1)]
public class CityTile : Tile
{
    public City City { get; private set; }

    public string Type { get; private set; }

    public void FinishConstruction(City city, string type)
    {
        Debug.Log("City Tile Construction Finished: " + type);
        City = city;
        Type = type;
    }
}
