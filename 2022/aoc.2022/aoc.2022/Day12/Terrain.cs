using System.Collections.Generic;
using System.Linq;

namespace Aoc.Day12;

public record Pos(int X, int Y);
public record Elevation(int Value, Pos Pos);
public partial class Terrain
{
    private readonly Dictionary<Pos, int> _elevation = new();
    private void SetElevation(Elevation elevation)
    {
        _elevation[elevation.Pos] = elevation.Value;
    }

    public IEnumerable<Elevation> GetNeighbouringElevation(Elevation e)
    {
        var p = e.Pos;
        return new[]
        {
            ElevationAt(p with { Y = p.Y - 1 }),
            ElevationAt(p with { X = p.X + 1 }),
            ElevationAt(p with { Y = p.Y + 1 }),
            ElevationAt(p with { X = p.X - 1 })
        }
            .Where(e => e is not null)
            .Select(e => e!);
    }

    public IEnumerable<Elevation> GetAllElevations => _elevation.Select(x => new Elevation(x.Value, x.Key));

    private Elevation? ElevationAt(Pos p)
    {
        return _elevation.TryGetValue(p, out var e) 
            ? new Elevation(e, p) 
            : null;
    }
}