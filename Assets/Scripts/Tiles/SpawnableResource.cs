using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawnable Resource", menuName = "ScriptableObjects/SpawnableResource", order = 1)]
public class SpawnableResource : ScriptableObject
{
    public string resourceID;
    public float minDistance;
    public float maxDistance;
    public int count;
    public string[] validTerrainNames;
}