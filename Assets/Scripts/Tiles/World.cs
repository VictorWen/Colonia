﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.UIElements;

// The interface for the terrain map
// Generates and accesses the terrain
// TODO: Organize World class
public class World : MonoBehaviour
{
    public Grid grid;
    public Tilemap terrain;
    public Tilemap cities;
    public Tilemap movement;

    [Header("Generation Settings")]
    public bool autoUpdate;
    public int seed;
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

    [Header("Terrain Tiles")]
    public TerrainData[] tiles;
    public BiomeTileSet[] biomes;

    [Header("Resource Generation")]
    public BiomeResources[] biomeResources;
    public ResourceIconScript iconPrefab;

    private Dictionary<Vector3Int, float> fertility;
    private Dictionary<Vector3Int, float> richness;

    //TODO: fix/organize this
    public Dictionary<Vector3Int, string> ResourceTiles { get; private set; }
    private Vector3Int[][] biomeChunks;
    private System.Random rand;

    private void Awake()
    {
        GenerateWorld();
        // Generate Resources
        ResourceTiles = ResourceGeneration.GenerateResources(this, biomeChunks, rand, biomeResources, iconPrefab);
    }

    [System.Serializable]
    public struct TerrainData
    {
        [Header("Generation")]
        [Range(0, 1)]
        public float height;
        public TerrainTile tile;
        public bool waterAdjacent;
    }

    public void GenerateWorld()
    {
        terrain.ClearAllTiles();
        Array.Sort(tiles, (x1, x2) => x1.height.CompareTo(x2.height));
        foreach (BiomeTileSet b in biomes)
        {
            Array.Sort(b.tiles, (x1, x2) => x1.height.CompareTo(x2.height));
        }

        rand = new System.Random(seed);
        int size = (worldRadius * 2 + 1) * (chunkRadius * 2 + 1);

        //TODO: better fertility and richness => create better biome terrain
        fertility = new Dictionary<Vector3Int, float>();
        richness = new Dictionary<Vector3Int, float>();

        // Borrowed from Sebastian Lague
        float[,] heightLevels = new float[size, size];
        float amplitude = 1f;
        float frequency = 1f;
        for (int i = 0; i < octaves; i++)
        {
            float offset = rand.Next(-100000, 100000);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float sampleX = (x - size / 2f) / scale * frequency + offset;
                    float sampleY = (y - size / 2f) / scale * frequency + offset;
                    heightLevels[x, y] += amplitude * (Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1);
                }
            }

            amplitude *= persistence;
            frequency *= lucanarity;
        }
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                heightLevels[x, y] = Mathf.InverseLerp(-octaveAmplitude, octaveAmplitude, heightLevels[x, y]);
            }
        }



        Vector2[] centers = GetChunkCenters(chunkRadius, worldRadius);
        biomeChunks = new Vector3Int[centers.Length][];
        int p = 0;
        // Assign Tiles
        foreach (Vector2 center in centers)
        {
            // Determine biome
            TerrainData[] biomeTiles = tiles;
            //Vector3 tilePos = grid.CellToWorld(pos);
            float distance = Vector2.Distance(center, new Vector2(0, 0)) / (1.5f * chunkRadius);

            if (distance >= 0.5 * worldRadius)
            {
                float angle = Mathf.Atan2(center.x, center.y);
                if (angle < 0)
                {
                    angle += 2 * Mathf.PI;
                }
                float interval = 2 * Mathf.PI / biomes.Length;
                float lower = -interval / 2;
                int index = 0;
                while (angle > lower && angle >= lower + interval)
                {
                    lower += interval;
                    index++;
                }
                biomeTiles = biomes[index % biomes.Length].tiles;
            }

            Vector3Int[] chunk = CreateChunk(center, chunkRadius);
            biomeChunks[p++] = chunk;
            foreach (Vector3Int pos in chunk)
            {
                int x = pos.x;
                int y = pos.y;

                float height = heightLevels[x + size / 2, y + size / 2];

                // Determine if adjacent/close to water
                bool waterAdjacent = false;
                for (int dx = -1; !waterAdjacent && dx <= 1; dx++)
                {
                    for (int dy = -1; !waterAdjacent && dy <= 1; dy++)
                    {
                        if (0 <= x + dx + size / 2 && x + dx + size / 2 < size && 0 <= y + dy + size / 2 && y + dy + size / 2 < size &&
                            heightLevels[x + dx + size / 2, y + dy + size / 2] <= biomeTiles[0].height)
                        {
                            waterAdjacent = true;
                        }
                    }
                }

                float noise = (float)(rand.NextDouble() * 2 - 1);
                // Match height to TileBase
                for (int i = 0; i < biomeTiles.Length; i++)
                {
                    TerrainData t = biomeTiles[i];
                    if (!t.waterAdjacent && height <= t.height)
                    {
                        terrain.SetTile(pos, t.tile);
                            
                        if (!fertility.ContainsKey(pos)) 
                            fertility.Add(pos, GetRandomAspect(rand, t.tile.baseFertility)); 
                        if (!richness.ContainsKey(pos))
                            richness.Add(pos, GetRandomAspect(rand, t.tile.baseRichness));

                        break;
                    }
                    else if (waterAdjacent && t.waterAdjacent && height <= t.height)
                    {
                        terrain.SetTile(pos, t.tile);

                        if (!fertility.ContainsKey(pos))
                            fertility.Add(pos, GetRandomAspect(rand, t.tile.baseFertility));
                        if (!richness.ContainsKey(pos))
                            richness.Add(pos, GetRandomAspect(rand, t.tile.baseRichness));

                        break;
                    }
                }
            }
            //terrain.SetTile(grid.WorldToCell(center), null);
        }
    }

    private float GetRandomAspect(System.Random rand, float baseAspect)
    {
        return baseAspect + (float)(rand.NextDouble() - 0.5) * baseAspect;
    }

    public float IsReachable(float moves, Vector3Int destination)
    {
        TerrainTile t = (TerrainTile)terrain.GetTile(destination);
        if (t != null && !t.impassable && moves >= t.movementCost)
        {
            return t.movementCost;
        }
        return -1;
    }

    // Creates meta-hexagon chunk
    private Vector3Int[] CreateChunk(Vector2 chunkCenter, int radius)
    {
        int area = 3 * radius * (radius + 1) + 1;
        Vector3Int[] tiles = new Vector3Int[area];
       
        // Fill mid line
        tiles[0] = grid.WorldToCell(chunkCenter);

        Vector2 center = chunkCenter;
        int index = 1;
        for (int i = 1; i <= radius; i++)
        {
            tiles[index] = grid.WorldToCell(i * new Vector2(-1, 0) + center);
            index++;
            tiles[index] = grid.WorldToCell(i * new Vector2(1, 0) + center);
            index++;
        }

        Vector2 topEdge = new Vector2(0.5f - radius, 0.75f) + center;
        Vector2 lowerEdge = new Vector2(0.5f - radius, -0.75f) + center;
        for (int layer = 0; layer < radius; layer++)
        {
            // Move to next layer and fill line
            for (int i = 0; i < radius * 2 - layer; i++)
            {
                tiles[index] = grid.WorldToCell(i * new Vector2(1, 0) + topEdge);
                index++;
                tiles[index] = grid.WorldToCell(i * new Vector2(1, 0) + lowerEdge);
                index++;
            }
            topEdge += new Vector2(0.5f, 0.75f);
            lowerEdge += new Vector2(0.5f, -0.75f);
        }

        return tiles;
    }

    private Vector2[] GetChunkCenters(int chunkRadius, int worldRadius)
    {
        int area = 3 * worldRadius * (worldRadius + 1) + 1;
        Vector2[] tiles = new Vector2[area];

        // Fill mid line
        Vector2 center = new Vector2(0, 0);
        tiles[0] = center;

        int index = 1;
        for (int i = 1; i <= worldRadius; i++)
        {
            tiles[index] = chunkRadius * (i * new Vector2(0, -1.5f) + center);
            index++;
            tiles[index] = chunkRadius * (i * new Vector2(0, 1.5f) + center);
            index++;
        }

        Vector2 right = new Vector2(1.5f, 0.75f - worldRadius * 1.5f) + center;
        Vector2 left = new Vector2(-1.5f, 0.75f - worldRadius * 1.5f) + center;
        for (int layer = 0; layer < worldRadius; layer++)
        {
            // Move to next layer and fill line
            for (int i = 0; i < worldRadius * 2 - layer; i++)
            {
                tiles[index] = (chunkRadius) * (i * new Vector2(0, 1.5f) + right);
                index++;
                tiles[index] = (chunkRadius) * (i * new Vector2(0, 1.5f) + left);
                index++;
            }
            right += new Vector2(1.5f, 0.75f);
            left += new Vector2(-1.5f, 0.75f);
        }

        return tiles;
    }

    public float GetFertilityAtTile(Vector3Int tilePos)
    {
        return fertility[tilePos];
    }

    public float GetRichnessAtTile(Vector3Int tilePos)
    {
        return richness[tilePos];
    }
}
