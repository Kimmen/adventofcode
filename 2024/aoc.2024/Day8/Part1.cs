using System.Collections.Concurrent;
using Spectre.Console;

namespace Aoc.Day8;

public class Part1 : IChallenge
{
    private string _inputExt = "txt";

    private long? _expectedResult = 348;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 14;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput).ToArray();
        (int Width, int Height) boundery = (lines.First().Length, lines.Length);
        var antennas = BuildAntennasMap(lines);
        var antiNodes = new HashSet<Point>();

        foreach (var frequency in antennas.Keys)
        {
            var antennasForFrequency = antennas[frequency];

            var distances = CalculateDistances(antennasForFrequency).ToArray();
            var antiNodesForFrequency = CalculateAntiNodes(distances);

            foreach (var antiNode in antiNodesForFrequency)
            {
                if (IsInBoundary(antiNode, boundery))
                {
                    antiNodes.Add(antiNode);    
                }
            }
        } 

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Unique anti nodes: ");
        PrintResult(antiNodes.Count);
    }

    private static bool IsInBoundary(Point point, (int Width, int Height) boundary)
    {
        return point is { X: >= 0, Y: >= 0 } && point.X < boundary.Width && point.Y < boundary.Height;
    }

    private static IEnumerable<Point> CalculateAntiNodes((Point A, Point B, ManhattanDistance Distance)[] distances)
    {
        foreach (var d in distances)
        {
            var (a, b, distance) = d;
            var signX = Math.Sign(a.X - b.X);
            var signY = Math.Sign(a.Y - b.Y);
            
            var antiNodeA = new Point(a.X + distance.X * signX, a.Y + distance.Y * signY);
            var antiNodeB = new Point(b.X - distance.X * signX, b.Y - distance.Y * signY);
            
            yield return antiNodeA;
            yield return antiNodeB;
        }
    }

    private static IEnumerable<(Point A, Point B, ManhattanDistance Distance)> CalculateDistances(List<Point> antennasForFrequency)
    {
        for (var i = 0; i < antennasForFrequency.Count; i++)
        {
            for (var j = i + 1; j < antennasForFrequency.Count; j++)
            {
                var a = antennasForFrequency[i];
                var b = antennasForFrequency[j];
                var distance = new ManhattanDistance(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
                yield return (a, b, distance);
            }
        }
    }

    private static ConcurrentDictionary<char, List<Point>> BuildAntennasMap(string[] lines)
    {
        var antennas = new ConcurrentDictionary<char, List<Point>>();

        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if (char.IsDigit(c) || char.IsLower(c) || char.IsUpper(c))
                {
                    var positions = antennas.GetOrAdd(c, []);
                    positions.Add(new Point(x, y));
                }
            }
        }
        
        return antennas;
    }

    public record struct Point(int X, int Y);
    public record struct ManhattanDistance(int X, int Y);

    private void PrintResult(long result)
    {
        var color = _expectedResult.HasValue ? "green" : "yellow";
        if (_expectedResult.HasValue && result != _expectedResult)
        {
            color = "red";
        }

        AnsiConsole.MarkupLine($"[{color} bold]{result}[/]");
    }
}