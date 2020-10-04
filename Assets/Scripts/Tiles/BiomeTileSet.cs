using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Biome Tile Set", order = 1)]
public class BiomeTileSet : ScriptableObject
{
    public WorldTerrain.TerrainData[] tiles;
}
