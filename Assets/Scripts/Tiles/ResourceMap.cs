using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Move to World class
public class ResourceMap
{
    public Dictionary<Vector3Int, ResourceIconScript> Icons { get; private set; }
    public Dictionary<Vector3Int, string> ResourceLocations { get; private set; }

    private readonly World world;
    private readonly ResourceIconScript iconPrefab;
    private readonly bool enableVision;

    public ResourceMap(World world, BiomeResources[] biomeResources, ResourceIconScript iconPrefab, bool enableVision = true)
    {
        this.world = world;
        this.iconPrefab = iconPrefab;
        this.enableVision = enableVision;

        if (Icons != null) // leftover code?
            foreach (ResourceIconScript icon in Icons.Values)
                Object.DestroyImmediate(icon.gameObject);

        Icons = new Dictionary<Vector3Int, ResourceIconScript>();
        ResourceLocations = new Dictionary<Vector3Int, string>();

        // Iterate through each BiomeResources
        foreach (BiomeResources biome in biomeResources)
            SpawnBiomeResources(biome);
    }

    private void SpawnBiomeResources(BiomeResources biome)
    {
        // Find the corresponding biome
        // Iterate through all tiles and sort each one by terrain
        Dictionary<string, List<Vector3Int>> terrainTiles = world.GetBiomeTiles(biome.biomeIndex);

        // Iterate through all SpawnableResource and match to corresponding terrain and filtering distance
        foreach (SpawnableResource resource in biome.resources)
        {
            int count = 0;
            HashSet<int> check = new HashSet<int>();
            List<Vector3Int> validTiles = new List<Vector3Int>();

            GameObject folder = new GameObject(resource.name);
            folder.transform.SetParent(world.transform);

            foreach (string validName in resource.validTerrainNames)
                validTiles.AddRange(terrainTiles[validName]);

            // Randomly select Spawnableresource->count number of tiles and place resources
            while (count < resource.count && count < validTiles.Count && check.Count < validTiles.Count)
                count += PlaceResourceRandomly(validTiles, check, resource, folder);
        }
    }

    private int PlaceResourceRandomly(List<Vector3Int> validTiles, HashSet<int> check, SpawnableResource resource, GameObject folder)
    {
        int val = 0;
        int index = world.RNG.Next(0, validTiles.Count);
        if (!check.Contains(index))
        {
            float distance = Vector3Int.Distance(validTiles[index], new Vector3Int(0, 0, 0));
            if (distance >= resource.minDistance && distance <= resource.maxDistance)
            {
                PlaceResource(validTiles[index], resource, folder);
                val = 1;
            }
            check.Add(index);
        }
        return val;
    }

    private void PlaceResource(Vector3Int position, SpawnableResource resource, GameObject folder)
    {
        ResourceLocations.Add(position, resource.resourceID);

        // Add ResourceIcon GameObject
        ResourceIconScript icon = Object.Instantiate(iconPrefab);
        icon.transform.position = world.CellToWorld(position);
        icon.transform.position += new Vector3(-0.5f, 0.25f);
        icon.SetResource(resource.resourceID);
        Icons.Add(position, icon);
        icon.transform.SetParent(folder.transform, true);
        icon.gameObject.SetActive(!enableVision);
    }
}
