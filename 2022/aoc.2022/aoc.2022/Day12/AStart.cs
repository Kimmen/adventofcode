using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Day12;

internal class AStar
{
    public static IEnumerable<Elevation> DetermineShortestPath(Terrain terrain, Elevation start, Elevation end)
    {
        var nearestToStart = new Dictionary<Elevation, Elevation>();
        var visited = new HashSet<Elevation>();
        var distanceFromStart = new Dictionary<Elevation, int>
        {
            [start] = 0
        };
        var heuristicsToEnd = new ConcurrentDictionary<Elevation, int>()
        {
            [start] = CalculateHeuristicsToEnd(start, end)
        };

        var prioQueue = new List<Elevation>
        {
            start
        };

        var current = start;
        while (prioQueue.Any() || current != end)
        {
            prioQueue = prioQueue
                .OrderBy(x => distanceFromStart[x] + heuristicsToEnd.GetOrAdd(x, x => CalculateHeuristicsToEnd(x, end)))
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
                nearestToStart.TryAdd(elevation, current);

                if (!prioQueue.Contains(elevation))
                {
                    prioQueue.Add(elevation);
                }
            }

            visited.Add(current);
        }

        var shortestPath = new List<Elevation> { end };
        current = end;
        while (!nearestToStart.ContainsKey(current))
        {
            current = nearestToStart[current];
            shortestPath.Add(current);
        }

        return shortestPath;
    }

    private static int DetermineCost(Elevation current, Elevation elevation)
    {
        return elevation.Value - current.Value < 2 
            ? 1 
            : 10;
    }

    private static int CalculateHeuristicsToEnd(Elevation current, Elevation end)
    {
        var distX = Math.Abs(end.Pos.X - current.Pos.X);
        var distY = Math.Abs(end.Pos.Y - current.Pos.Y);
        var heuristics = Convert.ToInt32(Math.Sqrt(distX * distX + distY * distY) * 1000);

        return heuristics;
    }
}