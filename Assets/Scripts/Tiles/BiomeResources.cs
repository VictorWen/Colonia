using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Biome Resources", menuName = "ScriptableObjects/Biome Resources", order = 1)]
public class BiomeResources : ScriptableObject
{
    public SpawnableResource[] resources;
    public int biomeIndex;
}