using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Move to World class
public class ResourceMap
{
    public Dictionary<Vector3Int, ResourceIconScript> Icons { get; private set; }
    public Dictionary<Vector3Int, string> ResourceLocations { get; private set; }

    public ResourceMap(World world, BiomeResources[] biomeResources, ResourceIconScript iconPrefab, bool enableVision = true)
    {
        if (Icons != null)
        {
            foreach (ResourceIconScript icon in Icons.Values)
            {
                Object.DestroyImmediate(icon.gameObject);
            }
        }

        Icons = new Dictionary<Vector3Int, ResourceIconScript>();
        ResourceLocations = new Dictionary<Vector3Int, string>();

        // Iterate through each BiomeResources
        foreach (BiomeResources biome in biomeResources)
        {
            // Find the corresponding biome
            // Iterate through all tiles and sort each one by terrain
            Dictionary<string, List<Vector3Int>> terrainTiles = new Dictionary<string, List<Vector3Int>>();

            foreach (Vector3Int position in world.BiomeChunks[biome.biomeIndex])
            {
                string terrainName = world.GetTerrainTile(position).name;
                if (!terrainTiles.ContainsKey(terrainName))
                {
                    terrainTiles.Add(terrainName, new List<Vector3Int>());
                }
                terrainTiles[terrainName].Add(position);
            }

            // Iterate through all SpawnableResource and match to corresponding terrain and filtering distance
            foreach (SpawnableResource resource in biome.resources)
            {
                int count = 0;
                HashSet<int> check = new HashSet<int>();
                List<Vector3Int> validTiles = new List<Vector3Int>();

                GameObject folder = new GameObject(resource.name);
                folder.transform.SetParent(world.transform);

                foreach (string validName in resource.validTerrainNames)
                {
                    validTiles.AddRange(terrainTiles[validName]);
                }

                // Randomly select Spawnableresource->count number of tiles and place resources
                while (count < resource.count && count < validTiles.Count && check.Count < validTiles.Count)
                {
                    int index = world.RNG.Next(0, validTiles.Count);
                    if (!check.Contains(index))
                    {
                        float distance = Vector3Int.Distance(validTiles[index], new Vector3Int(0, 0, 0));
                        if (distance >= resource.minDistance && distance <= resource.maxDistance)
                        {
                            ResourceLocations.Add(validTiles[index], resource.resourceID);

                            // Add ResourceIcon GameObject
                            ResourceIconScript icon = Object.Instantiate(iconPrefab);
                            icon.transform.position = world.grid.CellToWorld(validTiles[index]);
                            icon.transform.position += new Vector3(-0.5f, 0.25f);
                            icon.SetResource(resource.resourceID);
                            Icons.Add(validTiles[index], icon);
                            icon.transform.SetParent(folder.transform, true);
                            icon.gameObject.SetActive(!enableVision);

                            count++;
                        }
                        check.Add(index);
                    }
                }
            }
        }
    }
}
