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
            Dictionary<string, List<Vector3Int>> terrainTiles = world.GetBiomeTiles(0); // TODO: implement spawners for other biomes

            foreach (NPCSpawnerSO spawner in spawners)
            {
                int count = 0;
                HashSet<int> check = new HashSet<int>();
                List<Vector3Int> validTiles = new List<Vector3Int>();

                foreach (string validName in spawner.validTerrainNames)
                    validTiles.AddRange(terrainTiles[validName]);

                while (count < spawner.count && count < validTiles.Count && check.Count < validTiles.Count)
                    count += PlaceSpawnerRandomly(world, validTiles, check, spawner);
            }
        }

        private int PlaceSpawnerRandomly(World world, List<Vector3Int> validTiles, HashSet<int> check, NPCSpawnerSO spawner)
        {
            int val = 0;
            int index = world.RNG.Next(0, validTiles.Count);
            if (!check.Contains(index))
            {
                float distance = Vector3Int.Distance(validTiles[index], new Vector3Int(0, 0, 0));
                if (distance >= spawner.minDistance && distance <= spawner.maxDistance)
                {
                    Spawners.Add(new NPCSpawner(spawner.unitID, validTiles[index], spawner.spawnThreshold, spawner.spawnLimit));
                    world.PlaceSpawnerTile(validTiles[index]);
                    val = 1;
                }
                check.Add(index);
            }
            return val;
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
