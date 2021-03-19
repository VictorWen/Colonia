using System.Collections.Generic;
using Tiles;
using UnityEngine;

/// <summary>
/// Finds all the reachable tiles from a starting position within one turn given a movement speed
/// </summary>
public class BFSPathfinder
{
    public HashSet<Vector3Int> Reachables { get; private set; }
    private readonly Dictionary<Vector3Int, Vector3Int> priorPosition;

    public BFSPathfinder(Vector3Int start, int speed, IWorld world, bool checkUnits = false)
    {
        Reachables = new HashSet<Vector3Int>();
        priorPosition = new Dictionary<Vector3Int, Vector3Int>();

        List<Vector3Int> queue = new List<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, float> moves = new Dictionary<Vector3Int, float>();
        queue.Add(start);
        visited.Add(start);
        Reachables.Add(start);
        moves.Add(start, 0);

        while (queue.Count > 0)
        {
            foreach (Vector3Int gridTilePos in world.GetAdjacents(queue[0]))
            {
                float currentMoves = moves[queue[0]];
                float cost = world.IsReachable(speed - currentMoves, gridTilePos, checkUnits);
                float moveCost = cost + currentMoves;
                if (!visited.Contains(gridTilePos) && cost >= 0)
                {
                    visited.Add(gridTilePos);

                    Reachables.Add(gridTilePos);
                    priorPosition.Add(gridTilePos, queue[0]);

                    queue.Add(gridTilePos);
                    moves.Add(gridTilePos, moveCost);
                }
                else if (visited.Contains(gridTilePos) && cost >= 0 && moveCost < moves[gridTilePos])
                {
                    moves[gridTilePos] = moveCost;
                    priorPosition[gridTilePos] = queue[0];
                }
            }
            queue.RemoveAt(0);
        }
    }

    public LinkedList<Vector3Int> GetPathTo(Vector3Int target)
    {
        if (!Reachables.Contains(target))
            return null;
        LinkedList<Vector3Int> path = new LinkedList<Vector3Int>();
        Vector3Int last = target;
        path.AddFirst(target);
        while (priorPosition.ContainsKey(last))
        {
            last = priorPosition[last];
            path.AddFirst(last);
        }
        return path;
    }

}
