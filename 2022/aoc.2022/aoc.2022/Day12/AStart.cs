using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Day12;

internal class AStar
{
    public static IEnumerable<Elevation> DetermineShortestPath(Terrain terrain, Elevation[] starts, Elevation end)
    {
        var nearestToStart = new Dictionary<Elevation, Elevation>();
        var visited = new HashSet<Elevation>();
        var distanceFromStart = starts.ToDictionary(x => x, x => 0);
        var heuristicsToEnd = new ConcurrentDictionary<Elevation, int>(
            starts.Select(x => new KeyValuePair<Elevation, int>(x, CalculateManhattanDistanceToEnd(x, end))));

        var prioQueue = new List<Elevation>(starts);

        var current = starts.First();
        while (prioQueue.Any() && current != end)
        {
            prioQueue = prioQueue
                .OrderBy(x => distanceFromStart[x] + heuristicsToEnd.GetOrAdd(x, x => CalculateManhattanDistanceToEnd(x, end)))
                .ToList();
            current = prioQueue.First();
            prioQueue.Remove(current);
            
            var connectedElevations = terrain
                .GetNeighbouringElevation(current)
                .OrderBy(e => DetermineCost(current, e));

            foreach (var elevation in connectedElevations)
            {
                if (visited.Contains(elevation))
                {
                    continue;
                }

                var cost = distanceFromStart[current] + DetermineCost(current, elevation);
                if (distanceFromStart.TryGetValue(elevation, out var value) && value <= cost)
                {
                    continue;
                }
                
                distanceFromStart[elevation] = cost;
                nearestToStart[elevation] = current;

                if (!prioQueue.Contains(elevation))
                {
                    prioQueue.Add(elevation);
                }
            }

            visited.Add(current);
        }

        var shortestPath = new List<Elevation> { end };
        current = end;
        while (nearestToStart.ContainsKey(current))
        {
            current = nearestToStart[current];
            shortestPath.Add(current);
        }

        shortestPath.Reverse();

        return shortestPath;
    }

    private static int DetermineCost(Elevation current, Elevation elevation)
    {
        return elevation.Value - current.Value < 2 
            ? 1 
            : 1000;
    }

    private static int CalculateManhattanDistanceToEnd(Elevation current, Elevation end)
    {
        var distX = Math.Abs(end.Pos.X - current.Pos.X);
        var distY = Math.Abs(end.Pos.Y - current.Pos.Y);
        var heuristics = distX + distY;

        return heuristics;
    }
}