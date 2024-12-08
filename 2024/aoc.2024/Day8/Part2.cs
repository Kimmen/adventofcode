using System.Collections.Concurrent;
using System.Xml;
using Spectre.Console;

namespace Aoc.Day8;

public class Part2 : IChallenge
{
    private string _inputExt = "txt";

    private long? _expectedResult = null;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 34;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput).ToArray();
        var boundary = new Boundary(lines.First().Length, lines.Length);
        var antennas = BuildAntennasMap(lines);
        var antiNodes = new HashSet<Point>();

        foreach (var frequency in antennas.Keys)
        {
            var antennasForFrequency = antennas[frequency];

            var distances = CalculateDistances(frequency, antennasForFrequency).ToArray();
            var antiNodesForFrequency = CalculateAntiNodes(distances, boundary);

            foreach (var antiNode in antiNodesForFrequency)
            {
                antiNodes.Add(antiNode);
            }
        }
        
        AnsiConsole.Cursor.SetPosition(0, boundary.Height);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Unique anti-nodes: ");
        PrintResult(antiNodes.Count);
    }

    private static IEnumerable<Point> CalculateAntiNodes((Point A, Point B, ManhattanDistance Distance)[] distances, Boundary boundary)
    {
        foreach (var d in distances)
        {
            var (a, b, distance) = d;
            var signX = Math.Sign(a.X - b.X);
            var signY = Math.Sign(a.Y - b.Y);

            foreach (var node in CalculateAntiNodePath(boundary, a, p =>  new Point(p.X + distance.X * signX, p.Y + distance.Y * signY)))
            {
                AnsiConsole.Cursor.SetPosition(node.X + 1, node.Y + 1);
                AnsiConsole.MarkupInterpolated($"[maroon]#[/]");
                Thread.Sleep(100);
                yield return node;
            }
            
            foreach (var node in CalculateAntiNodePath(boundary, b, p =>  new Point(p.X - distance.X * signX, p.Y - distance.Y * signY)))
            {
                AnsiConsole.Cursor.SetPosition(node.X + 1, node.Y + 1);
                AnsiConsole.MarkupInterpolated($"[maroon]#[/]");
                Thread.Sleep(100);
                yield return node;
            }
        }
    }

    private static IEnumerable<Point> CalculateAntiNodePath(Boundary boundary, Point p, Func<Point, Point> calcNextPoint)
    {
        yield return p;
        
        while (true)
        {
            p = calcNextPoint(p);
            if (!boundary.IsInside(p))
            {
                break;
            }
            yield return p;
        }
    }

    private static IEnumerable<(Point A, Point B, ManhattanDistance Distance)> CalculateDistances(char frequency,
        List<Point> antennasForFrequency)
    {
        for (var i = 0; i < antennasForFrequency.Count; i++)
        {
            for (var j = i + 1; j < antennasForFrequency.Count; j++)
            {
                
                var a = antennasForFrequency[i];
                var b = antennasForFrequency[j];
                var distance = new ManhattanDistance(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
                
                AnsiConsole.Cursor.SetPosition(a.X + 1, a.Y + 1);
                AnsiConsole.MarkupInterpolated($"[green]{frequency}[/]");
                AnsiConsole.Cursor.SetPosition(b.X + 1, b.Y + 1);
                AnsiConsole.MarkupInterpolated($"[green]{frequency}[/]");
                Thread.Sleep(100);
                
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
                AnsiConsole.Markup($"{c}");
                if (char.IsDigit(c) || char.IsLower(c) || char.IsUpper(c))
                {
                    var positions = antennas.GetOrAdd(c, []);
                    positions.Add(new Point(x, y));
                }
            }
            
            AnsiConsole.WriteLine();
        }
        
        return antennas;
    }

    public record struct Point(int X, int Y);
    public record struct ManhattanDistance(int X, int Y);

    public record struct Boundary(int Width, int Height)
    {
        public bool IsInside(Point p)
        {
            return p is { X: >= 0, Y: >= 0 } && p.X < Width && p.Y < Height;
        }
    };

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