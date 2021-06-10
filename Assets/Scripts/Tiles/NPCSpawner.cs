using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Units;

namespace Tiles
{
    public class NPCSpawner
    {
        public string UnitID { private set; get; }
        public Vector3Int Position { private set; get; }

        private int spawnPoints;
        private int spawnThreshold;

        public NPCSpawner(string unitID, Vector3Int position, int threshold)
        {
            UnitID = unitID;
            Position = position;

            spawnPoints = 0;
            spawnThreshold = threshold;
        }

        public void OnNextTurn(GameMaster game)
        {
            spawnPoints += CalulcateSpawnGrowth(game.World);

            if (spawnPoints >= spawnThreshold)
            {
                spawnPoints -= spawnThreshold;
                SpawnNPCUnit(game);    
            }
        }

        private void SpawnNPCUnit(GameMaster game)
        {
            game.SpawnUnitEntity(UnitID, "TEST SPAWNED ENTITY", Position);
        }

        private int CalulcateSpawnGrowth(World world)
        {
            int growth = 1;
            foreach (Vector3Int tile in world.GetTilesInRange(Position, 3))
            {
                UnitEntity unit = world.UnitManager.GetUnitAt<UnitEntity>(tile);
                if (unit == null)
                    continue;

                if (unit.IsPlayerControlled)
                    growth += 2;
                else
                    growth--;
            }
            if (growth < 0)
                growth = 0;

            return growth;
        }
    }
}
