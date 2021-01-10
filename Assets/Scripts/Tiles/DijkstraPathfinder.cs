using System;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraPathfinder
{
    public LinkedList<Vector3Int> Path { get; private set; }

    private readonly bool checkUnits;

    private class Location
    {
        public Location parent;
        public Vector3Int position;
        public float distance;

        public Location(Vector3Int position, Location parent, float distance)
        {
            this.position = position;
            this.parent = parent;
            this.distance = distance;
        }

        public static int Compare(Location a, Location b)
        {
            if (a.position.Equals(b.position))
                return 0;
            if (a.distance <= b.distance)
                return -1;
            else
                return 1;
        }
    }

    // TODO: add labeling of path so it is known during which turn a given move will occur.
    public DijkstraPathfinder(Vector3Int start, float maxMoveSpeed, Vector3Int target, World world, bool checkUnits = false)
    {
        this.checkUnits = checkUnits;

        // Runs Dijkstras, finding the shortest path to the target position and storing it.
        Dictionary<Vector3Int, Location> locations = new Dictionary<Vector3Int, Location>();
        SortedSet<Location> queue = new SortedSet<Location>(Comparer<Location>.Create(Location.Compare));
        Location startLoc = new Location(start, null, 0);
        locations.Add(start, startLoc);
        queue.Add(startLoc);
        
        Location targetLoc = null;
        int count = 0;
        while (queue.Count > 0)
        {
            Location source = queue.Min;
            if (source == null)
            {
                Debug.LogError("Invalid");
            }
            if (source.position.Equals(target))
            {
                targetLoc = source;
                break;
            }
            foreach (Vector3Int adj in world.GetAdjacents(source.position))
            {
                float cost = world.IsReachable(maxMoveSpeed, adj, this.checkUnits);
                if (cost >= 0)
                {
                    if (locations.ContainsKey(adj) && locations[adj].distance > source.distance + cost)
                    {
                        locations[adj].distance = source.distance + cost;
                        locations[adj].parent = source;
                    }
                    else if (!locations.ContainsKey(adj))
                    {
                        Location adjLoc = new Location(adj, source, cost + source.distance);
                        queue.Add(adjLoc);
                        locations.Add(adj, adjLoc);
                    }
                }
            }
            if (!queue.Remove(queue.Min))
            {
                // Debug
                int x = Location.Compare(source, queue.Min);
                Debug.LogError(x);
            }
            if (count > 1000) //Check to prevent infinite loop
                break;
            count++;
        }

        Path = new LinkedList<Vector3Int>();
        Location last = targetLoc;
        while (last != null && !last.position.Equals(start))
        {
            Path.AddFirst(last.position);
            last = last.parent;
        }
    }

    /// <summary>
    /// Returns the path to the target reachable with a given movement speed within a turn
    /// </summary>
    public LinkedList<Vector3Int> NextMovement(int movementSpeed, World world)
    {
        LinkedList<Vector3Int> movement = new LinkedList<Vector3Int>();
        float moves = movementSpeed;
        if (Path.Count == 0)
            return null;
        float cost = world.IsReachable(moves, Path.First.Value, checkUnits);
        if (cost < 0)
            return null; //First tile is not reachable, retry pathfinding!
        
        while (cost >= 0 && Path.Count > 0) 
        {
            moves -= cost;
            movement.AddLast(Path.First.Value);
            Path.RemoveFirst();
            if (Path.Count > 0)
            {
                cost = world.IsReachable(moves, Path.First.Value, checkUnits);
            }
            else
                cost = -1;
        }
        return movement;
    }
}