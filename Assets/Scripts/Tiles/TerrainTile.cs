using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Terrain Tile", order = 1)]
public class TerrainTile : Tile
{
    public bool impassable;
    public float movementCost;
    public float baseFertility;
    public float baseRichness;

    public string Biome { get; set; }
}
