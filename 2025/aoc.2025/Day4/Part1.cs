using Spectre.Console;

namespace Aoc.Day4;

public partial class Part1 : IChallenge
{
    private string _input = "txt";
    private long? _expectedResult = 21139440284L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;

    public void UseDevInput()
    {
        _input = "dev.txt";
        _expectedResult = 13L;
    }

    public void RefreshRate(long rate)
    {
        _refreshRate = rate;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_input}";

    private const char Roll = '@';
    private const char Empty = '.';
    private const char AccessibleRoll = 'x';

    public void Run()
    {
        var sum = 0L;

        var lines = InputReader.ReadLinesFromResource(PuzzleInput).ToArray();
        var grid = BuildGrid(lines);
        var canvasSize = grid.GetLength(0);
        var canvas = new Canvas(Math.Min(canvasSize, 100), Math.Min(canvasSize, 100));

        AnsiConsole.Live(canvas).Start(ctx =>
        {
            var currentPosition = (X: 0, Y: 0);
            CanvasHelpers.RenderViewPort(canvas, grid, (-1, -1), currentPosition, ColorPicker, ctx);

            for (var y = 0; y < grid.GetLength(0); y++)
            {
                for (var x = 0; x < grid.GetLength(1); x++)
                {
                    currentPosition = (X: x, Y: y);
                    try
                    {
                        var loc = grid[y, x];
                        if (loc == '.')
                        {
                            continue;
                        }

                        var adjacentRolls = GetAdjacentRolls((x, y), grid);
                        var isAccessible = adjacentRolls.Length < 4;

                        if (isAccessible)
                        {
                            grid[y, x] = AccessibleRoll;
                            sum++;
                        }
                    }
                    finally
                    {
                        if (DoRefresh())
                        {
                            CanvasHelpers.RenderViewPort(canvas, grid, currentPosition, currentPosition, ColorPicker, ctx);
                        }
                    }
                }
            }

            CanvasHelpers.RenderViewPort(canvas, grid, currentPosition, currentPosition, ColorPicker, ctx);
        });

        PrintResult(sum);
    }

    private (int x, int y)[] GetAdjacentRolls((int x, int y) pos, char[,] grid)
    {
        const int stepsOutwards = 1;
        var positions = new List<(int x, int y)>();

        for(var dx = -stepsOutwards; dx <= stepsOutwards; dx++)
        {
            for(var dy = -stepsOutwards; dy <= stepsOutwards; dy++)
            {
                if(dx == 0 && dy == 0)
                {
                    continue;
                }

                var newX = pos.x + dx;
                var newY = pos.y + dy;
                var outsideBounds = newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1);
                if (outsideBounds)
                {
                    continue;
                }
                if(grid[newY, newX] == Empty)
                {
                    continue;
                }

                positions.Add((newX, newY));
            }
        }

        return positions.ToArray();
    }

    private static Color ColorPicker(char arg, bool isCurrent)
    {
        if (isCurrent)
        {
            return Color.White;
        }

        return arg switch
        {
            Roll => Color.Maroon,
            Empty => Color.DarkSlateGray1,
            AccessibleRoll => Color.DarkGreen,
            _ => Color.Black
        };
    }

    private static char[,] BuildGrid(string[] lines)
    {
        var first = lines.First();
        var board = new char[lines.Length, first.Length];

        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                board[y, x] = line[x];
            }
        }

        return board;
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