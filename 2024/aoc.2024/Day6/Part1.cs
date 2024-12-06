using Spectre.Console;

namespace Aoc.Day6;

public class Part1 : IChallenge
{
    private string _inputExt = "txt";
    private long? _expectedResult = 5177;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 41;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput).ToArray();

        var canvas = new Canvas(30, 30);
        var uniquePositions = 0;

        AnsiConsole.Live(canvas).Start(ctx =>
        {
            var board = BuildBoard(lines, out var currentPos);

            Visit(currentPos, board);
            var selectedDirection = 0;
            var currentCanvasViewport = DetermineCanvasViewport(canvas, currentPos);
            RenderBoard(canvas, board, currentCanvasViewport);
            ctx.Refresh();

            while (true)
            {

                var currentDirection = _directions[selectedDirection];
                var nextPos = (X: currentPos.X + currentDirection.X, Y: currentPos.Y + currentDirection.Y);
                if (OutOfBoard(nextPos, board))
                {
                    uniquePositions++;
                    break;
                }

                if(board[nextPos.X, nextPos.Y] == '#')
                {
                    selectedDirection = (selectedDirection + 1) % _directions.Length;
                    currentDirection = _directions[selectedDirection];
                    nextPos = (X: currentPos.X + currentDirection.X, Y: currentPos.Y + currentDirection.Y);
                }

                RenderPart(canvas, board, currentPos, nextPos, currentDirection.Symbol, currentCanvasViewport, ctx);
                uniquePositions += Visit(nextPos, board) ? 0 : 1;

                currentPos = nextPos;
            }
        });

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Number of steps: ");
        PrintResult(uniquePositions);
    }

    private (int Top, int Left) DetermineCanvasViewport(Canvas canvas, (int X, int Y) startPosition)
    {
        var left = Convert.ToInt32(Math.Floor((double)startPosition.X / canvas.Width)) * canvas.Width;
        var top = Convert.ToInt32(Math.Floor((double)startPosition.Y / canvas.Height)) * canvas.Height;

        return (top, left);
    }

    private void RenderPart(Canvas canvas, char[,] board, (int X, int Y) currentPosition, (int X, int Y) nextPos, char currentPositionSymbol, (int Top, int Left) viewport, LiveDisplayContext ctx)
    {
        var newViewport = DetermineCanvasViewport(canvas, nextPos);
        if(newViewport != viewport)
        {
            viewport = newViewport;
            RenderBoard(canvas, board, viewport);
        }

        var viewPortX = currentPosition.X - viewport.Left;
        var viewPortY = currentPosition.Y - viewport.Top;
        PrintPosition(canvas, (viewPortX, viewPortY), board[currentPosition.X, currentPosition.Y]);
        viewPortX = nextPos.X - viewport.Left;
        viewPortY = nextPos.Y - viewport.Top;
        PrintPosition(canvas, (viewPortX, viewPortY), currentPositionSymbol);

        ctx.Refresh();
    }

    private static void RenderBoard(Canvas canvas, char[,] board, (int Top, int Left) newViewport)
    {
        for (var y = 0; y < canvas.Height; y++)
        {
            var boardY = y + newViewport.Top;
            for (var x = 0; x < canvas.Width; x++)
            {
                var boardX = x + newViewport.Left;

                if(boardX < board.GetLength(0) && boardY < board.GetLength(1))
                {
                    var symbol = board[boardX, boardY];
                    PrintPosition(canvas, (x, y), symbol);
                }

                if(boardX >= board.GetLength(0))
                {
                    x = canvas.Width;
                }
            }

            if (boardY >= board.GetLength(1))
            {
                y = canvas.Height;
            }
        }
    }

    private static bool Visit((int X, int Y) startPosition, char[,] board)
    {
        var alreadyVisited = board[startPosition.X, startPosition.Y] == ':';
        board[startPosition.X, startPosition.Y] = ':';

        return alreadyVisited;
    }

    private static bool OutOfBoard((int X, int Y) pos, char[,] board)
    {
        return pos.X < 0 || pos.Y < 0 || board.GetLength(0) <= pos.X || board.GetLength(1) <= pos.Y;
    }

    private static char[,] BuildBoard(string[] lines, out (int X, int Y) startPosition)
    {
        (int X, int Y) currentBoardPosition = (0, 0);
        startPosition = currentBoardPosition;
        var first = lines.First();
        var board = new char[first.Length, lines.Length];
        foreach (var line in lines)
        {
            foreach (var c in line)
            {
                var symbol = c;
                if (symbol == '^')
                {
                    symbol = '.';
                    startPosition = currentBoardPosition;
                }


                board[currentBoardPosition.X, currentBoardPosition.Y] = symbol;
                currentBoardPosition = (currentBoardPosition.X + 1, currentBoardPosition.Y);
            }

            currentBoardPosition = (0, currentBoardPosition.Y + 1);
        }

        return board;
    }

    private const char Up = '\u2191'; // ↑
    private const char Down = '\u2193'; // ↓
    private const char Left = '\u2190'; // ←
    private const char Right = '\u2192'; // →

    private static readonly Dictionary<char, Color> BoardCharColors = new()
    {
        { '#', Color.Maroon },
        { '.', Color.Grey93 },
        { ':', Color.LightSlateGrey },
        { Up, Color.Gold1 },
        { Down, Color.Gold3_1 },
        { Left, Color.DarkGoldenrod },
        { Right, Color.LightGoldenrod1 },
    };

    private static (int X, int Y, char Symbol)[] _directions = [
        (X: 0, Y:-1, Symbol: Up), //up
        (X: 1, Y:0, Symbol: Right), //right
        (X: 0, Y:1, Symbol: Down), //down
        (X: -1, Y:0, Symbol: Left) //left
    ];

    private static void PrintPosition(Canvas canvas, (int X, int Y) position, char c)
    {
        if(position.X < 0 || position.Y < 0 || position.X >= canvas.Width || position.Y >= canvas.Height)
        {
            return;
        }

        canvas.SetPixel(position.X, position.Y, BoardCharColors[c]);
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