using System.Collections.Generic;
using System.Linq;

namespace Aoc.Day16;

internal class BFS
{
    public static IList<Valve> GetShortestPath(Valve start, Valve end, IDictionary<string, Valve> valves) 
    {
        var visited = new HashSet<string>();
        var parent = new Dictionary<string, Valve>();
        var queue = new Queue<Valve>();

        visited.Add(start.Name);
        queue.Enqueue(start);
    
        while (queue.Any())
        {
            var current = queue.Dequeue();

            var adjacentValves = current.ConnectedValves
                .Where(v => !visited.Contains(v))
                .Select(v => valves[v])
                .ToList();

            foreach (var av in adjacentValves)
            {
                parent[av.Name] = current;
                if (av.Name != end.Name)
                {
                    visited.Add(av.Name);
                    queue.Enqueue(av);
                }
                else
                {
                    queue.Clear();
                    break;
                }
            }
        }

        var path = new List<Valve> { end };
        var c = end;
        while (parent.TryGetValue(c.Name, out c))
        {
            path.Add(c);
        }

        return path;
    }
}
