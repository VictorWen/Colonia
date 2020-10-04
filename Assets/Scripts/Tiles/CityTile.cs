using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/City Tile", order = 1)]
public class CityTile : Tile
{
    public GameObject cityGUI;

    private City city;

    public void SetCity(City c)
    {
        this.city = c;
    }

    public void OpenCityGUI()
    {
        cityGUI.SetActive(true);
    }
}
