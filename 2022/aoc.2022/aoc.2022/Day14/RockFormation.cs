using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc.Day14;

internal partial class RockFormation
{
    private static readonly VertexComparer _comparer = new();
    internal static (HashSet<(int x, int y)> Rocks, (int x, int y) Min, (int x, int y) Max) Plot(string input)
    {
        var plottedShapes = new HashSet<(int x, int y)>();
        var shapes = InputReader
            .ReadLinesFromResource(input)
            .Select(ParseShape);

        //Determine boundery
        var minX = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;

        var processVertex = ((int x, int y) v) =>
        {
            if(v.x < minX) minX = v.x;
            if(v.x > maxX) maxX = v.x;
            if(v.y > maxY) maxY = v.y;

            plottedShapes.Add(v);
        };


        foreach (var shape in shapes)
        {
            PlotShape(shape, processVertex);
        }

        return (plottedShapes, (minX, 0), (maxX, maxY));
    }

    private static IEnumerable<(int x, int y)> ParseShape(string line)
    {
        var matches = EdgeFactory().Matches(line);
        foreach (var match in matches.AsEnumerable())
        {
            yield return (int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));
        }
    }

    private static void PlotShape(IEnumerable<(int x, int y)> shapes, Action<(int x, int y)> process)
    {
        var shapesArray = shapes.ToArray();

        for (int i = 1; i < shapesArray.Length; i++)
        {
            var curr = shapesArray[i - 1];
            var end = shapesArray[i];

            process(curr);
            while (curr != end)
            {
                var dx = Math.Sign(end.x - curr.x);
                var dy = Math.Sign(end.y - curr.y);
                curr = (curr.x + dx, curr.y + dy);

                process(curr);
            }
        }
    }

    [GeneratedRegex("(?'x'\\d+),(?'y'\\d+)", RegexOptions.Compiled)]
    private static partial Regex EdgeFactory();
}

public class VertexComparer : IComparer<(int x, int y)>
{
    public int Compare((int x, int y) a, (int x, int y) b)
    {
        if (a.x < b.x)
        {
            return -1;
        }

        if (a.x > b.x)
        {
            return 1;
        }

        if (a.y < b.y)
        {
            return -1;
        }
        
        if (a.y > b.y)
        {
            return 1;
        }

        return 0;
    }
}