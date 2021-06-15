using UnityEngine;
using System.Collections.Generic;
using Units;

namespace Tiles
{
    public interface IWorld
    {
        float IsReachable(float moves, Vector3Int destination, bool checkUnits = false);

        float GetCombatModifierAt(Vector3Int tile);

        void UpdatePlayerVision(HashSet<Vector3Int> oldUnitVisibles, HashSet<Vector3Int> newUnitVisibles);

        HashSet<Vector3Int> GetLineOfSight(Vector3Int start, int range);

        List<Vector3Int> GetAdjacents(Vector3Int tile);

        List<List<Vector3Int>> GetRangeList(Vector3Int start, int range);

        HashSet<Vector3Int> GetTilesInRange(Vector3Int start, int range);

        UnitEntityManager UnitManager { get; }
    }
}
