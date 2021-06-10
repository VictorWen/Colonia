using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tiles;

[CreateAssetMenu(fileName = "World Gen Config", menuName = "ScriptableObjects/World Generation Config", order = 1)]
public class WorldGenerationConfig : ScriptableObject
{
    [Header("Vision")]
    public Tile cloud;
    public Tile fog;

    [Header("Generation Settings")]
    public int chunkRadius;
    public int worldRadius;
    [Range(0.5f, 25f)]
    public float scale;
    [Range(1, 16)]
    public int octaves;
    [Range(0.1f, 2f)]
    public float persistence;
    [Range(0.1f, 1f)]
    public float lucanarity;
    [Range(0.5f, 4f)]
    public float octaveAmplitude;

    [System.Serializable]
    public struct TerrainData
    {
        [Header("Generation")]
        [Range(0, 1)]
        public float height;
        public TerrainTile tile;
        public bool waterAdjacent;
    }

    [Header("Terrain Tiles")]
    public TerrainData[] tiles;
    public BiomeTileSet[] biomes;

    [Header("Resource Generation")]
    public BiomeResources[] biomeResources;
    public ResourceIconScript iconPrefab;

    [Header("Spawner Generation")]
    public NPCSpawnerSO[] spawners;
}
