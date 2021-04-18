using System;
using System.Collections.Generic;
using Tiles;
using Units;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cities;
using Cities.Construction;

// The interface for the terrain map
// Generates and accesses the terrain
// TODO: Organize World class
public class World : MonoBehaviour, IWorld
{
    public Grid grid;
    [SerializeField] private Tilemap terrain;
    [SerializeField] private Tilemap cities;
    [SerializeField] private Tilemap movement;

    [Header("Vision")]
    public bool enableFogOfWar;
    [SerializeField] private Tilemap vision;
    public Dictionary<Vector3Int, int> Visible { get; private set; }

    [Header("Generation Settings")]
    public bool autoUpdate;
    public int seed;

    [SerializeField] private WorldGenerationConfig config = null;

    //TODO: fix/organize this
    public ResourceMap ResourceMap { get; private set; }
    public Vector3Int[][] BiomeChunks { get; private set; }
    public System.Random RNG { get; private set; }

    private Dictionary<Vector3Int, float> fertility;
    private Dictionary<Vector3Int, float> richness;

    public UnitEntityManager UnitManager { get; private set; }

    private void Awake()
    {
        GenerateWorld();
        // Generate Resources
        ResourceMap = new ResourceMap(this, config.biomeResources, config.iconPrefab, enableFogOfWar);
        UnitManager = new UnitEntityManager();
    }

    // UnitEntityManager wrappers =======================
    public void ExecuteNPCTurns(GameMaster game)
    {
        //UnitManager.ExecuteNPCTurns(game);
    }

    public void UnitsOnNextTurn(GameMaster game)
    {
        UnitManager.NextTurn(game);
    }

    // World Vision ===========================
    public void AddFogOfWar(Vector3Int position)
    {
        if (Visible.ContainsKey(position))
        {
            Visible[position]--;
            if (Visible[position] == 0)
            {
                Visible.Remove(position);
                if (enableFogOfWar)
                {
                    vision.SetTile(position, config.fog);
/*                    UnitEntity unitAt = GetUnitAt(position);
                    if (unitAt != null && !unitAt.PlayerControlled)
                        unitAt.HideScript();*/
                }
            }
        }
    }

    public void RevealTerraIncognita(Vector3Int position)
    {
        TileBase tile = vision.GetTile(position);
        if (tile != null && tile.name == "Cloud")
        {
            vision.SetTile(position, config.fog);
            if (ResourceMap.Icons.ContainsKey(position))
                ResourceMap.Icons[position].gameObject.SetActive(true);
        }
    }

    public void RevealFogOfWar(Vector3Int position)
    {
        if (Visible.ContainsKey(position))
        {
            Visible[position]++;
        }
        else
        {
            Visible.Add(position, 1);
            vision.SetTile(position, null);
/*            UnitEntity unitAt = GetUnitAt(position);
            if (unitAt != null && !unitAt.PlayerControlled)
                unitAt.ShowScript();*/
        }
    }
    
    /// <summary>
    /// Approximates the line of sight to each hexagon within a given radius by placing
    /// each hexagon within a angle bucket and backtracking to find any collisions
    /// with buckets in an earlier layer
    /// </summary>
    public HashSet<Vector3Int> GetLineOfSight(Vector3Int start, int range)
    {
        //Debug.Log("LINE OF SIGHT");
        range += (int)((TerrainTile)terrain.GetTile(start)).sightBonus;
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
                float angle = margin * i;
                float leftRay = angle - margin / 2;
                float rightRay = angle + margin / 2;
                //Debug.Log(tile + "\t" + leftRay + "\t" + rightRay);

                // Backtrack
                bool clear = true;
                float pastSightCost = 1;
                //float pastSightCost = ((TerrainTile)terrain.GetTile(tile)).sightCost;
                //float pastSightCost = 0;
                for (int j = r - 1; j > 0; j--)
                {
                    float layerMargin = Mathf.PI * 2 / (j * 6 * 2);
                    Vector3Int left = rangeList[j][Mathf.CeilToInt(leftRay / layerMargin) / 2 % (j * 6)];
                    Vector3Int right = rangeList[j][Mathf.CeilToInt(rightRay / layerMargin) / 2 % (j * 6)];
                    if (!(sight.Contains(left) || sight.Contains(right)))
                    {
                        clear = false;
                        break;
                    }

                    // Determine sight cost
                    float leftSightCost = 10000;
                    float rightSightCost = 10000;
                    if (sight.Contains(left) && terrain.GetTile(left) != null)
                        leftSightCost = ((TerrainTile)terrain.GetTile(left)).sightCost;
                    if (sight.Contains(right) && terrain.GetTile(right) != null)
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

    // Utility Tile Grabbers =====================================
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
    // ===========================================================

    // World Movement =========================================
    /// <summary>
    /// Calculates whether a tile is reachable with a given movement speed
    /// </summary>
    /// <param name="moves">The remaing movement speed available</param>
    /// <param name="destination">The destination tile</param>
    /// <param name="checkUnits">Whether to check if the destination tile has a unit on it or not</param>
    /// <returns>Return the movement cost to get to the destination tile if it is reachable, otherwise returns -1</returns>
    public float IsReachable(float moves, Vector3Int destination, bool checkUnits = false)
    {
        TerrainTile t = (TerrainTile)terrain.GetTile(destination);
        if (t != null && !t.impassable && moves >= t.movementCost && (!checkUnits || UnitManager.GetUnitAt(destination) == null))
        {
            return t.movementCost;
        }
        return -1;
    }

    public float GetMovementCost(Vector3Int position)
    {
        if (terrain.GetTile(position) != null)
        {
            return ((TerrainTile)terrain.GetTile(position)).movementCost;
        }
        return 0f;
    }
    
    // World Generation ==========================================
    public void GenerateWorld()
    {
        Visible = new Dictionary<Vector3Int, int>();
        terrain.ClearAllTiles();
        vision.ClearAllTiles();
        Array.Sort(config.tiles, (x1, x2) => x1.height.CompareTo(x2.height));
        foreach (BiomeTileSet b in config.biomes)
        {
            Array.Sort(b.tiles, (x1, x2) => x1.height.CompareTo(x2.height));
        }

        RNG = new System.Random(seed);
        int size = (config.worldRadius * 2 + 1) * (config.chunkRadius * 2 + 1);

        //TODO: better fertility and richness => create better biome terrain
        fertility = new Dictionary<Vector3Int, float>();
        richness = new Dictionary<Vector3Int, float>();

        // Borrowed from Sebastian Lague
        float[,] heightLevels = new float[size, size];
        float amplitude = 1f;
        float frequency = 1f;
        for (int i = 0; i < config.octaves; i++)
        {
            float offset = RNG.Next(-100000, 100000);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float sampleX = (x - size / 2f) / config.scale * frequency + offset;
                    float sampleY = (y - size / 2f) / config.scale * frequency + offset;
                    heightLevels[x, y] += amplitude * (Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1);
                }
            }

            amplitude *= config.persistence;
            frequency *= config.lucanarity;
        }
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                heightLevels[x, y] = Mathf.InverseLerp(-config.octaveAmplitude, config.octaveAmplitude, heightLevels[x, y]);
            }
        }

        Vector2[] centers = GetChunkCenters(config.chunkRadius, config.worldRadius);
        BiomeChunks = new Vector3Int[centers.Length][];
        int p = 0;
        // Assign Tiles
        foreach (Vector2 center in centers)
        {
            // Determine biome
            WorldGenerationConfig.TerrainData[] biomeTiles = config.tiles;
            //Vector3 tilePos = grid.CellToWorld(pos);
            float distance = Vector2.Distance(center, new Vector2(0, 0)) / (1.5f * config.chunkRadius);

            if (distance >= 0.5 * config.worldRadius)
            {
                float angle = Mathf.Atan2(center.x, center.y);
                if (angle < 0)
                {
                    angle += 2 * Mathf.PI;
                }
                float interval = 2 * Mathf.PI / config.biomes.Length;
                float lower = -interval / 2;
                int index = 0;
                while (angle > lower && angle >= lower + interval)
                {
                    lower += interval;
                    index++;
                }
                biomeTiles = config.biomes[index % config.biomes.Length].tiles;
            }

            Vector3Int[] chunk = CreateChunk(center, config.chunkRadius);
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
                    WorldGenerationConfig.TerrainData t = biomeTiles[i];
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
                if (enableFogOfWar)
                    vision.SetTile(pos, config.cloud);
            }
            //terrain.SetTile(grid.WorldToCell(center), null);
        }
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

    private float GetRandomAspect(System.Random rand, float baseAspect)
    {
        return baseAspect + (float)(rand.NextDouble() - 0.5) * baseAspect;
    }

    public float GetFertilityAtTile(Vector3Int tilePos)
    {
        return fertility[tilePos];
    }

    public float GetRichnessAtTile(Vector3Int tilePos)
    {
        return richness[tilePos];
    }

    public float GetCombatModifierAt(Vector3Int tilePos)
    {
        return ((TerrainTile)terrain.GetTile(tilePos)).combatModifier;
    }
    
    // Terrain Tiles =============================================
    public TerrainTile GetTerrainTile(Vector3Int position)
    {
        return (TerrainTile)terrain.GetTile(position);
    }

    // City Tiles =================================================
    public ConstructedTile GetConstructedTile(Vector3Int position)
    {
        return (ConstructedTile)cities.GetTile(position);
    }

    public void StartConstructionOfCityTile(ConstructedTile tile, Vector3Int position)
    {
        tile.StartConstruction();
        PlaceConstructedTile(position, tile);
        cities.SetTileFlags(position, TileFlags.None);
        cities.SetColor(position, new Color(0.5f, 0.5f, 0.5f));
    }

    public void FinishConstructionOfCityTile(City city, ConstructedTileProject project, Vector3Int position, ConstructedTileProject upgradee = null)
    {
        ConstructedTile tile = GetConstructedTile(position);
        tile.FinishConstruction(city, project.ProjectType, project);
        cities.SetColor(position, new Color(1, 1, 1));

        if (upgradee != null)
        {
            //TODO: Manage replacing old constructed tile after upgrade
            project.OnUpgrade(upgradee);
        }
    }

    public void PlaceConstructedTile(Vector3Int position, ConstructedTile tile)
    {
        cities.SetTile(position, tile);
    }

    // Movement Tiles ================================================
    public void SetMovementTile(Vector3Int position, TileBase tile)
    {
        movement.SetTile(position, tile);
    }
}
