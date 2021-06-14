using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tiles
{
    [CreateAssetMenu(fileName = "NPC Spawner", menuName = "ScriptableObjects/NPC Spawner", order = 1)]
    public class NPCSpawnerSO : ScriptableObject
    {
        public string unitID;
        public string[] validTerrainNames;
        public int count;
        public float minDistance;
        public float maxDistance;

        public int growthMultiplier = 1;
        public int spawnThreshold;
        public int spawnLimit = 3;
    }
}
