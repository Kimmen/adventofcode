using System.Collections.Immutable;
using Spectre.Console;

namespace Aoc.Day4;

public class Part2 : IChallenge
{
    private static readonly Style CommonStyle = Style.Plain.Foreground(Color.LightSkyBlue3_1);
    private static readonly Style AStyle = Style.Plain.Foreground(Color.Maroon).Decoration(Decoration.Bold);
    private static readonly Style CorrectStyle = Style.Plain.Foreground(Color.Green).Decoration(Decoration.Bold);

    private string _inputExt = "txt";
    private long? _expectedResult = 1875;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 9L;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput).ToImmutableArray();
        var columnCount = lines.First().Length;
        var rowCount = lines.Length;

        var xmasCount = 0L;
        var grid = new char[rowCount, columnCount];
        var aPositions = new HashSet<(int Row, int Column)>(); ;

        for (var row = 0; row < lines.Length; row++)
        {
            for (var column = 0; column < lines[row].Length; column++)
            {
                var c = lines[row][column];
                grid[row, column] = c;
                var isA = c == 'A';

                if (isA)
                {
                    aPositions.Add((row, column));
                }
            }
        }

        var mas = "MAS".ToCharArray();
        var sam = "SAM".ToCharArray();

        var lastPrintedRow = -1;
        for (var i = 0; i < aPositions.Count; i++)
        {
            var (row, column) = aPositions.ElementAt(i);
            if(lastPrintedRow < row)
            {
                lastPrintedRow = PrintSection(grid, row);
            }

            var middle = grid[row, column];
            var topLeft = GetFromGrid(grid, row - 1, column - 1);
            var bottomRight = GetFromGrid(grid, row + 1, column + 1);
            var topRight = GetFromGrid(grid, row - 1, column + 1);
            var bottomLeft = GetFromGrid(grid, row + 1, column - 1);

            var matchLeftDiagonal = MatchArray(mas, topLeft, middle, bottomRight) || MatchArray(sam, topLeft, middle, bottomRight);
            var matchRightDiagonal = MatchArray(mas, topRight, middle, bottomLeft) || MatchArray(sam, topRight, middle, bottomLeft);
            Thread.Sleep(1);

            if (matchLeftDiagonal && matchRightDiagonal)
            {
                MarkCurrent(column, row, middle, CorrectStyle);
                xmasCount++;
            }
            else
            {
                MarkCurrent(column, row, middle, CommonStyle);
            }
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("XMAS count: ");
        PrintResult(xmasCount);
    }

    private const int DisplayableRowsCount = 20;
    private static void MarkCurrent(int column, int row, char middle, Style selectStyle)
    {
        var cappedRow = row % DisplayableRowsCount;
        AnsiConsole.Cursor.SetPosition(column + 1, cappedRow + 1);
        AnsiConsole.Write(new Text(middle.ToString(), selectStyle));
        AnsiConsole.Cursor.SetPosition(column + 1, cappedRow + 1);
    }

    private static int PrintSection(char[,] grid, int row)
    {
        if (row % DisplayableRowsCount != 0)
        {
            return row;
        }

        AnsiConsole.Console.Clear();
        var maxRowIndex = Math.Min(row + DisplayableRowsCount, grid.GetLength(0));
        int lastRow;
        for (lastRow = row; lastRow < maxRowIndex; lastRow++)
        {
            for (var c = 0; c < grid.GetLength(1); c++)
            {
                var cValue = grid[lastRow, c];
                var style = cValue == 'A' ? AStyle : CommonStyle;

                AnsiConsole.Write(new Text(cValue.ToString(), style));
            }
            AnsiConsole.WriteLine($" {lastRow}");
        }

        AnsiConsole.WriteLine($"{row} -> {maxRowIndex} / {grid.GetLength(0)}");
        return lastRow - 1;
    }

    private static bool MatchArray(char[] compareTo, params char[] chars)
    {
        return compareTo.SequenceEqual(chars);
    }

    private static char GetFromGrid(char[,] grid, int row, int column)
    {
        if (row < 0 || column < 0 || row >= grid.GetLength(0) || column >= grid.GetLength(1))
        {
            return ' ';
        }

        return grid[row, column];
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