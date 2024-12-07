using Spectre.Console;

namespace Aoc.Day6;

public class Part2 : IChallenge
{
    private string _inputExt = "txt";
    private long? _expectedResult = null;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 6;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput).ToArray();
        var canvas = new Canvas(30, 30);
        var timeParadoxObstructionCount = 0;

        AnsiConsole.Live(canvas).Start(ctx =>
        {
            var board = BuildBoard(lines, out var currentPos);
            var viewport = DetermineCanvasViewport(canvas, currentPos);
            var selectedDirection = 0;
            
            Visit(currentPos, board, Directions[selectedDirection]);
            RenderBoard(canvas, board, viewport);
            ctx.Refresh();
            
            while (true)
            {   
                var mainRay = CastRay(board, currentPos, Directions[selectedDirection]);
                foreach (var pos in mainRay)
                {
                    var direction = Directions[selectedDirection];
                    viewport = RenderPart(canvas, board, currentPos, pos, direction.Symbol, viewport);
                    Visit(pos, board, direction);
                    
                    if (IsTimeParadoxObstruction(board, currentPos, NextDirection(selectedDirection)))
                    {
                        board[pos.X, pos.Y].Symbol = ';';
                        RenderPosition(canvas, pos, board[pos.X, pos.Y].Symbol);
                        timeParadoxObstructionCount++;
                    }
                    
                    currentPos = pos;
                    
                }
                ctx.Refresh();
                Thread.Sleep(100);
                
                if (OutOfBounds(NextPos(currentPos, Directions[selectedDirection]), board))
                {
                    break;
                }
                
                selectedDirection = NextDirection(selectedDirection);
            }
        });

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Number of steps: ");
        PrintResult(timeParadoxObstructionCount);
        
        Console.Read();
    }

    private static bool IsTimeParadoxObstruction(Tile[,] board, Pos startPosition, int directionIndex)
    {
        var hasVisited = new HashSet<(Pos Pos, char Direction)>();
        while (true)
        {
            
            var direction = Directions[directionIndex];
            var ray = CastRay(board, startPosition, direction);
            foreach (var pos in ray)
            {
                if (OutOfBounds(NextPos(pos, direction), board))
                {
                    return false;
                }
                
                var hasVisitedTestPath = hasVisited.Contains((pos, direction.Symbol));
                var hasVisitedMainPath = ray.Any(p => board[p.X, p.Y].Visited.Contains(direction.Symbol));
                if (hasVisitedMainPath || hasVisitedTestPath)
                {
                    return true;
                }
                
                hasVisited.Add((pos, direction.Symbol));
                startPosition = pos;
            }
            directionIndex = NextDirection(directionIndex);
        }
        
        
    }

    private static Pos NextPos(Pos pos, (int X, int Y, char Symbol) direction) => new(pos.X + direction.X, pos.Y + direction.Y);
    private static int NextDirection(int dir) => (dir + 1) % Directions.Length;

    private static List<Pos> CastRay(Tile[,] board, Pos position, (int X, int Y, char Symbol) direction)
    {
        var ray = new List<Pos>(100);
        while (CanGoOn(position))
        {
            ray.Add(position);
            position = NextPos(position, direction);
        }

        return ray;

        bool CanGoOn(Pos p) => !OutOfBounds(p, board) && !AtObstruction(p, board);
    }

    private static bool AtObstruction(Pos p, Tile[,] board) => board[p.X, p.Y].Symbol == '#';

    private static bool OutOfBounds(Pos p, Tile[,] board) => p.X < 0 || p.X >= board.GetLength(0) || p.Y < 0 || p.Y >= board.GetLength(1);

    private static Viewport DetermineCanvasViewport(Canvas canvas, Pos startPosition)
    {
        var left = Convert.ToInt32(Math.Floor((double)startPosition.X / canvas.Width)) * canvas.Width;
        var top = Convert.ToInt32(Math.Floor((double)startPosition.Y / canvas.Height)) * canvas.Height;

        return new Viewport(top, left);
    }

    private static Viewport RenderPart(Canvas canvas, Tile[,] board, Pos prevPos, Pos currentPos, char currentPosSymbol, Viewport viewport)
    {
        var newViewport = DetermineCanvasViewport(canvas, currentPos);
        if(newViewport != viewport)
        {
            viewport = newViewport;
            RenderBoard(canvas, board, viewport);
        }

        var viewPortX = prevPos.X - viewport.Left;
        var viewPortY = prevPos.Y - viewport.Top;
        RenderPosition(canvas, new Pos(viewPortX, viewPortY), board[prevPos.X, prevPos.Y].Symbol);
        viewPortX = currentPos.X - viewport.Left;
        viewPortY = currentPos.Y - viewport.Top;
        RenderPosition(canvas, new Pos(viewPortX, viewPortY), currentPosSymbol);
        
        return viewport;
    }

    private static void RenderBoard(Canvas canvas, Tile[,] board, Viewport newViewport)
    {
        for (var y = 0; y < canvas.Height; y++)
        {
            var boardY = y + newViewport.Top;
            for (var x = 0; x < canvas.Width; x++)
            {
                var boardX = x + newViewport.Left;

                if(boardX < board.GetLength(0) && boardY < board.GetLength(1))
                {
                    var symbol = board[boardX, boardY].Symbol;
                    RenderPosition(canvas, new Pos(x, y), symbol);
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

    private static void Visit(Pos startPosition, Tile[,] board, (int X, int Y, char Symbol) currentDirection)
    {
        var tile = board[startPosition.X, startPosition.Y]!;
        
        tile.Visited.Add(currentDirection.Symbol);
        if (tile.Symbol is '.')
        {
            tile.Symbol = ':';
        }
    }

    private static Tile[,] BuildBoard(string[] lines, out Pos startPosition)
    {
        var boardPos = new Pos(0, 0);
        startPosition = boardPos;
        var first = lines.First();
        var board = new Tile[first.Length, lines.Length];
        foreach (var line in lines)
        {
            foreach (var c in line)
            {
                var symbol = c;
                if (symbol == '^')
                {
                    symbol = '.';
                    startPosition = boardPos;
                }

                board[boardPos.X, boardPos.Y] = new Tile { Symbol = symbol };
                boardPos = boardPos with { X = boardPos.X + 1 };
            }

            boardPos = new Pos(0, boardPos.Y + 1);
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
        { ';', Color.LightSlateBlue },
        { Up, Color.Gold1 },
        { Down, Color.Gold3_1 },
        { Left, Color.DarkGoldenrod },
        { Right, Color.LightGoldenrod1 },
    };

    private static readonly (int X, int Y, char Symbol)[] Directions = [
        (X: 0, Y:-1, Symbol: Up), //up
        (X: 1, Y:0, Symbol: Right), //right
        (X: 0, Y:1, Symbol: Down), //down
        (X: -1, Y:0, Symbol: Left) //left
    ];

    private static void RenderPosition(Canvas canvas, Pos position, char c)
    {
        if(position.X < 0 || position.Y < 0 || position.X >= canvas.Width || position.Y >= canvas.Height)
        {
            return;
        }

        canvas.SetPixel(position.X, position.Y, BoardCharColors[c]);
    }

    private record struct Pos(int X, int Y);

    private class Tile
    {
        public char Symbol { get; set; }
        public HashSet<char> Visited { get; set; } = [];
    };
    private record struct Viewport(int Top, int Left);

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