using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//TODO: REMOVE CityMap
public class CityMap : MonoBehaviour
{
    public Grid grid;
    public Tilemap cities;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3Int tilePos = grid.WorldToCell(Input.mousePosition);
            CityTile tile = (CityTile)cities.GetTile(tilePos);
            tile.OpenCityGUI();
        }
    }
}
