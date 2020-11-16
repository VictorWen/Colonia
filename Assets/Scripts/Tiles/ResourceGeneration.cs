using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Move to World class
public class ResourceGeneration
{
    private static List<ResourceIconScript> icons = new List<ResourceIconScript>();

    public static Dictionary<Vector3Int, string> GenerateResources(World world, Vector3Int[][] biomeChunks, System.Random rand, BiomeResources[] biomeResources, ResourceIconScript iconPrefab)
    {
        Dictionary<Vector3Int, string> resourceLocations = new Dictionary<Vector3Int, string>();

        foreach (ResourceIconScript icon in icons)
        {
            Object.DestroyImmediate(icon.gameObject);
        }

        icons = new List<ResourceIconScript>();

        // Iterate through each BiomeResources
        foreach (BiomeResources biome in biomeResources)
        {
            // Find the corresponding biome
            // Iterate through all tiles and sort each one by terrain
            Dictionary<string, List<Vector3Int>> terrainTiles = new Dictionary<string, List<Vector3Int>>();
            
            foreach (Vector3Int position in biomeChunks[biome.biomeIndex])
            {
                string terrainName = world.terrain.GetTile(position).name;
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
                
                foreach (string validName in resource.validTerrainNames)
                {
                    validTiles.AddRange(terrainTiles[validName]);
                }

                // Randomly select Spawnableresource->count number of tiles and place resources
                while (count < resource.count && count < validTiles.Count && check.Count < validTiles.Count)
                {
                    int index = rand.Next(0, validTiles.Count);
                    if (!check.Contains(index))
                    {
                        float distance = Vector3Int.Distance(validTiles[index], new Vector3Int(0, 0, 0));
                        if (distance >= resource.minDistance && distance <= resource.maxDistance)
                        {
                            resourceLocations.Add(validTiles[index], resource.resourceID);
                            
                            // Add ResourceIcon GameObject
                            ResourceIconScript icon = Object.Instantiate(iconPrefab);
                            icon.transform.position = world.grid.CellToWorld(validTiles[index]);
                            icon.transform.position += new Vector3(-0.5f, 0.25f);
                            icon.SetResource(resource.resourceSprite);
                            icons.Add(icon);

                            count++;
                        }
                        check.Add(index);
                    }
                }
            }
        }

        return resourceLocations;
    }
}
