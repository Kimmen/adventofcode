using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day5;

public class Day5
{
    private readonly ITestOutputHelper output;

    public Day5(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Part1()
    {
        var rows = Helpers.ReadLinesFromResource("Aoc.Day5.input.txt")
            .ToList();

        var numberOfOverlappingPoints = rows
            .Select(AsLine)
            .Where(l => IsHorizontal(l) || IsVertical(l) )
            .SelectMany(AsPointSegment)
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Count();

        this.output.WriteLine($"Overlapping points: {numberOfOverlappingPoints}");
    }

    [Fact]
    public void Part2()
    {
        var rows = Helpers.ReadLinesFromResource("Aoc.Day5.input.txt")
            .ToList();

        var numberOfOverlappingPoints = rows
            .Select(AsLine)
            .SelectMany(AsPointSegment)
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Count();

        this.output.WriteLine($"Overlapping points: {numberOfOverlappingPoints}");
    }

    private static bool IsVertical((Point start, Point end) l)
    {
        return l.start.x == l.end.x;
    }

    private static bool IsHorizontal((Point start, Point end) l)
    {
        return l.start.y == l.end.y;
    }

    private (Point start, Point end) AsLine(string row)
    {
        static Point ParsePoint(string pointValue)
        {
            var numericalValues = pointValue.Split(",").Select(x => int.Parse(x)).ToList();
            return new Point(numericalValues.First(), numericalValues.Last());
        }
        var points = row.Split(" -> ").Select(ParsePoint);

        return (points.First(), points.Last());
    }

    private IEnumerable<Point> AsPointSegment((Point start, Point end) line)
    {
        var (start, end) = line;
        var vector = new Point(end.x - start.x, end.y - start.y);
        var magnitude = Math.Sqrt(vector.x * vector.x + vector.y * vector.y);
        var traverser = new Point(Convert.ToInt32(Math.Round(vector.x / magnitude)), Convert.ToInt32(Math.Round(vector.y / magnitude)));

        yield return start;
        var current = start with { };
        while (current != end)
        {
            var (x, y) = current;
            current = current with { x = x + traverser.x, y = y + traverser.y };
            yield return current;
        }
    }

    public record Point(int x, int y);

}
