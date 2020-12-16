using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Units;

// The interface for the terrain map
// Generates and accesses the terrain
// TODO: Organize World class
public class World : MonoBehaviour
{
    public Grid grid;
    public Tilemap terrain;
    public Tilemap cities;
    public Tilemap movement;
   
    [Header("Vision")]
    public Tilemap vision;
    public Tile cloud;
    public Tile fog;
    public Dictionary<Vector3Int, int> Visible { get; private set; }

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

    public UnitEntityManager UnitManager { get; private set; }

    //TODO: fix/organize this
    public ResourceMap ResourceMap { get; private set; }
    public Vector3Int[][] BiomeChunks { get; private set; }
    public System.Random RNG { get; private set; }

    private void Awake()
    {
        GenerateWorld();
        // Generate Resources
        ResourceMap = new ResourceMap(this, biomeResources, iconPrefab);
        UnitManager = new UnitEntityManager();
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
        Visible = new Dictionary<Vector3Int, int>();
        terrain.ClearAllTiles();
        Array.Sort(tiles, (x1, x2) => x1.height.CompareTo(x2.height));
        foreach (BiomeTileSet b in biomes)
        {
            Array.Sort(b.tiles, (x1, x2) => x1.height.CompareTo(x2.height));
        }

        RNG = new System.Random(seed);
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
            float offset = RNG.Next(-100000, 100000);
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
        BiomeChunks = new Vector3Int[centers.Length][];
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
            BiomeChunks[p++] = chunk;
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

                float noise = (float)(RNG.NextDouble() * 2 - 1);
                // Match height to TileBase
                for (int i = 0; i < biomeTiles.Length; i++)
                {
                    TerrainData t = biomeTiles[i];
                    if (!t.waterAdjacent && height <= t.height)
                    {
                        terrain.SetTile(pos, t.tile);
                            
                        if (!fertility.ContainsKey(pos)) 
                            fertility.Add(pos, GetRandomAspect(RNG, t.tile.baseFertility)); 
                        if (!richness.ContainsKey(pos))
                            richness.Add(pos, GetRandomAspect(RNG, t.tile.baseRichness));

                        break;
                    }
                    else if (waterAdjacent && t.waterAdjacent && height <= t.height)
                    {
                        terrain.SetTile(pos, t.tile);

                        if (!fertility.ContainsKey(pos))
                            fertility.Add(pos, GetRandomAspect(RNG, t.tile.baseFertility));
                        if (!richness.ContainsKey(pos))
                            richness.Add(pos, GetRandomAspect(RNG, t.tile.baseRichness));

                        break;
                    }
                }
                vision.SetTile(pos, cloud);
            }
            //terrain.SetTile(grid.WorldToCell(center), null);
        }
    }

    private float GetRandomAspect(System.Random rand, float baseAspect)
    {
        return baseAspect + (float)(rand.NextDouble() - 0.5) * baseAspect;
    }

    public void AddFogOfWar(Vector3Int position, UnitEntityManager manager)
    {
        if (Visible.ContainsKey(position))
        {
            Visible[position]--;
            if (Visible[position] == 0)
            {
                Visible.Remove(position);
                vision.SetTile(position, fog);
                if (manager.Positions.ContainsKey(position) && !manager.Positions[position].PlayerControlled)
                    manager.Positions[position].HideScript();
            }
        }
    }

    public void RevealTerraIncognita(Vector3Int position)
    {
        TileBase tile = vision.GetTile(position);
        if (tile != null && tile.name == "Cloud")
        {
            vision.SetTile(position, fog);
            if (ResourceMap.Icons.ContainsKey(position))
                ResourceMap.Icons[position].gameObject.SetActive(true);
        }
    }

    public void RevealFogOfWar(Vector3Int position, UnitEntityManager manager)
    {
        if (Visible.ContainsKey(position))
        {
            Visible[position]++;
        }
        else
        {
            Visible.Add(position, 1);
            vision.SetTile(position, null);
            if (manager.Positions.ContainsKey(position) && !manager.Positions[position].PlayerControlled)
                manager.Positions[position].ShowScript();
        }
    }

    public List<Vector3Int> GetAdjacents(Vector3Int tile)
    {
        List<Vector3Int> adjacents = new List<Vector3Int>();
        Vector3[] checks = new Vector3[] { new Vector3(-1, 0), new Vector3(-0.5f, 0.75f), new Vector3(0.5f, 0.75f), new Vector3(1, 0), new Vector3(0.5f, -0.75f), new Vector3(-0.5f, -0.75f) };
        foreach (Vector3 check in checks)
        {
            adjacents.Add(grid.WorldToCell(check + grid.CellToWorld(tile)));
        }
        return adjacents;
    }

    /// <summary>
    /// Approximates the line of sight to each hexagon within a given radius by placing
    /// each hexagon within a angle bucket and backtracking to find any collisions
    /// with buckets in an earlier layer
    /// </summary>
    public HashSet<Vector3Int> GetLineOfSight(Vector3Int start, int range)
    {
        //Debug.Log("LINE OF SIGHT");
        Vector3 worldStart = grid.CellToWorld(start);
        HashSet<Vector3Int> sight = new HashSet<Vector3Int>();
        List<List<Vector3Int>> rangeList = GetRangeList(start, range);
        sight.Add(start);
        for (int r = 1; r <= range; r++)
        {
            float margin = Mathf.PI * 2 / (r * 6);
            for (int i = 0; i < r * 6; i++)
            {
                // Calculate angles
                Vector3Int tile = rangeList[r][i];
                Vector3 worldTile = grid.CellToWorld(tile);
                Vector3 dVector = worldTile - worldStart;
                float angle = margin * i;
                float leftRay = angle - margin / 2;
                float rightRay = angle + margin / 2;
                //Debug.Log(tile + "\t" + leftRay + "\t" + rightRay);

                // Backtrack
                bool clear = true;
                float pastSightCost = 1;
                for (int j = r - 1; j > 0; j--)
                {
                    float layerMargin = Mathf.PI * 2 / (j * 6 * 2);
                    Vector3Int left = rangeList[j][Mathf.CeilToInt(leftRay / layerMargin) / 2 % (j*6)];
                    Vector3Int right = rangeList[j][Mathf.CeilToInt(rightRay / layerMargin) / 2 % (j*6)];
                    if (!(sight.Contains(left) || sight.Contains(right)))
                    {
                        clear = false;
                        break;
                    }

                    // Determine sight cost
                    float leftSightCost = 1000;
                    float rightSightCost = 1000;
                    if (sight.Contains(left))
                        leftSightCost = ((TerrainTile)terrain.GetTile(left)).sightCost;
                    if (sight.Contains(right))
                        rightSightCost = ((TerrainTile)terrain.GetTile(right)).sightCost;
                    pastSightCost += Mathf.Min(leftSightCost, rightSightCost);
                }
                if (clear && pastSightCost <= range)
                {   
                    sight.Add(tile);
                }
            }
        }
        return sight;
    }

    public List<List<Vector3Int>> GetRangeList(Vector3Int start, int range)
    {
        List<List<Vector3Int>> rangeList = new List<List<Vector3Int>>() { new List<Vector3Int>() { start } };
        Vector3 worldStart = grid.CellToWorld(start);
        
        for (int r = 1; r <= range; r++) {
            Vector3[] checks = new Vector3[] { new Vector3(-0.5f, 0.75f), new Vector3(-1, 0), new Vector3(-0.5f, -0.75f), new Vector3(0.5f, -0.75f), new Vector3(1, 0), new Vector3(0.5f, 0.75f)};
            List<Vector3Int> tiles = new List<Vector3Int>();
            Vector3 adder = r * new Vector3(1f, 0);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    tiles.Add(grid.WorldToCell(worldStart + adder));
                    adder += checks[i];
                }
            }
            rangeList.Add(tiles);
        }
        return rangeList;
    }

    public HashSet<Vector3Int> GetTilesInRange(Vector3Int start, int range)
    {
        HashSet<Vector3Int> tileRange = new HashSet<Vector3Int>
        {
            start
        };

        Vector3 worldStart = grid.CellToWorld(start);

        for (int i = 1; i <= range; i++)
        {
            tileRange.Add(grid.WorldToCell(i * new Vector3(-1, 0) + worldStart));
            tileRange.Add(grid.WorldToCell(i * new Vector3(1, 0) + worldStart));
        }

        Vector3 topEdge = new Vector3(0.5f - range, 0.75f) + worldStart;
        Vector3 lowerEdge = new Vector3(0.5f - range, -0.75f) + worldStart;
        for (int layer = 0; layer < range; layer++)
        {
            // Move to next layer and fill line
            for (int i = 0; i < range * 2 - layer; i++)
            {
                tileRange.Add(grid.WorldToCell(i * new Vector3(1, 0) + topEdge));
                tileRange.Add(grid.WorldToCell(i * new Vector3(1, 0) + lowerEdge));
            }
            topEdge += new Vector3(0.5f, 0.75f);
            lowerEdge += new Vector3(0.5f, -0.75f);
        }

        return tileRange;
    }

    public float IsReachable(float moves, Vector3Int destination, bool checkUnits = false)
    {
        TerrainTile t = (TerrainTile)terrain.GetTile(destination);
        if (t != null && !t.impassable && moves >= t.movementCost && (!checkUnits || !UnitManager.Positions.ContainsKey(destination)))
        {
            return t.movementCost;
        }
        return -1;
    }

    public float IsViewable(float moves, Vector3Int destination)
    {
        TerrainTile t = (TerrainTile)terrain.GetTile(destination);
        if (t != null && moves > 0)
        {
            return t.sightCost;
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
