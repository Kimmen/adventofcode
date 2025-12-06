using Spectre.Console;
using Spectre.Console.Rendering;

namespace Aoc.Day6;

public partial class Part1 : IChallenge
{
    private string _input = "txt";
    private long? _expectedResult = 5667835681547L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;
    
    const int ColumnsInViewPort = 10;

    public void UseDevInput()
    {
        _input = "dev.txt";
        _expectedResult = 4277556L;
    }

    public void RefreshRate(long rate)
    {
        _refreshRate = rate;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_input}";

    public void Run()
    {
        var sum = 0L;

        var (valuesGrid, operators) = BuildInputGrid();

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Calculating...", ctx =>
            {
                var columns = valuesGrid.GetLength(1);
                for (var c = 0; c < columns; c++)
                {
                    var numbers = GetNumberForColumn(valuesGrid, c);
                    var op = operators[c];
                    
                    
                    var total = op switch
                    {
                        '*' => numbers.Aggregate((total, n) => total * n),
                        '+' => numbers.Aggregate((total, n) => total + n),
                        _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
                    };
                
                    sum += total;
                    AnsiConsole.MarkupLineInterpolated($"Total is {total}, running sum is {sum}");
                    if (DoRefresh())
                    {
                        Thread.Sleep(300);
                    }
                }
            });
        
        PrintResult(sum);
    }

   
    private static long[] GetNumberForColumn(long[,] valuesGrid, int column)
    {
        var rows = valuesGrid.GetLength(0);
        var columnValues =  new long[rows];

        for (var r = 0; r < rows; r++)
        {
            columnValues[r] = valuesGrid[r, column];
        }
        
        return columnValues;
    }

    private (long[,] Values, char[] Operators) BuildInputGrid()
    {
        var lines = InputReader.ReadLinesFromResource(PuzzleInput);
        var valueLines = lines
            .Take(lines.Count - 1)
            .Select(l => l.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList())
            .ToArray();
        var operatorLine = lines.Last()
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(char.Parse)
            .ToArray();
        
        var yMax = valueLines.Length;
        var xMax = valueLines.First().Count;

        var grid = new long[yMax, xMax];
        for (var y = 0; y < valueLines.Length; y++)
        {
            var values = valueLines[y];
            for (int x = 0; x < values.Count; x++)
            {
                grid[y, x] = values[x];
            }
        }

        return (grid, operatorLine);
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

    private bool DoRefresh() => _refreshCount++ % _refreshRate == 0;
}