using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tiles
{
    public class SpawnerMap
    {
        public List<NPCSpawner> Spawners { private set; get; }

        public SpawnerMap(World world, NPCSpawnerSO[] spawners)
        {
            Spawners = new List<NPCSpawner>();
            Dictionary<string, List<Vector3Int>> terrainTiles = new Dictionary<string, List<Vector3Int>>();

            foreach (Vector3Int position in world.BiomeChunks[0])
            {
                string terrainName = world.GetTerrainTile(position).name;
                if (!terrainTiles.ContainsKey(terrainName))
                {
                    terrainTiles.Add(terrainName, new List<Vector3Int>());
                }
                terrainTiles[terrainName].Add(position);
            }

            foreach (NPCSpawnerSO spawner in spawners)
            {
                int count = 0;
                HashSet<int> check = new HashSet<int>();
                List<Vector3Int> validTiles = new List<Vector3Int>();

                foreach (string validName in spawner.validTerrainNames)
                {
                    validTiles.AddRange(terrainTiles[validName]);
                }

                while (count < spawner.count && count < validTiles.Count && check.Count < validTiles.Count)
                {
                    int index = world.RNG.Next(0, validTiles.Count);
                    if (!check.Contains(index))
                    {
                        float distance = Vector3Int.Distance(validTiles[index], new Vector3Int(0, 0, 0));
                        if (distance >= spawner.minDistance && distance <= spawner.maxDistance)
                        {
                            Spawners.Add(new NPCSpawner(spawner.unitID, validTiles[index], spawner.spawnThreshold));
                            count++;
                        }
                        check.Add(index);
                    }
                }
            }
        }

        public void NextTurn(GameMaster game)
        {
            foreach (NPCSpawner spawner in Spawners)
            {
                spawner.OnNextTurn(game);
            }
        }
    }
}
