using System.Collections.Immutable;
using Spectre.Console;

namespace Aoc.Day4;

public class Part1 : IChallenge
{
    private string _inputExt = "txt";
    private long? _expectedResult = 2536L;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 18L;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput).ToImmutableArray();
        var columnCount = lines.First().Length;
        var rowCount = lines.Length;
        var xmasCount = 0L;
        var grid = new char[rowCount, columnCount];
        var xPositions = new HashSet<(int Row, int Column)>();

        var xStyle = Style.Plain.Foreground(Color.Maroon).Decoration(Decoration.Bold);
        var selectStyle = Style.Plain.Foreground(Color.Orange3).Decoration(Decoration.Bold);
        var commonStyle = Style.Plain.Foreground(Color.LightSkyBlue3_1);

        for (var row = 0; row < lines.Length; row++)
        {
            for (var column = 0; column < lines[row].Length; column++)
            {
                var c = lines[row][column];
                grid[row, column] = c;
                var isX = c == 'X';

                if (isX)
                {
                    xPositions.Add((row, column));
                }

                var color = isX ? xStyle : commonStyle;


                AnsiConsole.Write(new Text(c.ToString(), color));
            }
            AnsiConsole.WriteLine();
        }

        Thread.Sleep(100);

        (int Rows, int Column)[] directions =
        [
            (0, 1), // down
            (0, -1), // up
            (1, 0), // right
            (-1, 0), // left
            (1, 1), // right down
            (-1, -1), // left up
            (-1, 1), // right up
            (1, -1) // left down
        ];

        var target = "XMAS".ToCharArray();

        for (var i = 0; i < xPositions.Count; i++)
        {

            var matchingXmasDirections = directions
                .Where(dir =>
                {
                    var (row, column) = xPositions.ElementAt(i);

                    var (dr, dc) = dir;
                    foreach (var c in target)
                    {
                        if (row < 0 || row >= rowCount || column < 0 || column >= columnCount)
                        {
                            return false;
                        }

                        var x = grid[row, column];
                        AnsiConsole.Cursor.SetPosition(column + 1, row + 1);
                        AnsiConsole.Write(new Text(x.ToString(), selectStyle));
                        Thread.Sleep(100);
                        AnsiConsole.Cursor.SetPosition(column + 1, row + 1);
                        AnsiConsole.Write(new Text(x.ToString(), xPositions.Contains((row, column)) ? xStyle : commonStyle));
                        if (x != c)
                        {
                            return false;
                        }

                        row += dr;
                        column += dc;
                    }

                    return true;
                })
                .ToArray();
            var count = matchingXmasDirections.Length;
            xmasCount += count;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("XMAS count: ");
        PrintResult(xmasCount);
    }

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