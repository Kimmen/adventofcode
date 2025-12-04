using Spectre.Console;

namespace Aoc;

public static class CanvasHelpers
{
    public static (int Top, int Left) RenderViewPort(
        Canvas canvas,
        char[,] board,
        (int Top, int Left) viewport,
        (int X, int Y) position,
        Func<char, bool, Color> boardColorMapper,
        LiveDisplayContext ctx)
    {
        var newViewport = DetermineCanvasViewport(canvas, position);
        if(newViewport != viewport)
        {
            viewport = newViewport;
            RenderBoard(canvas, board, viewport, boardColorMapper);
        }

        var viewPortX = position.X - viewport.Left;
        var viewPortY = position.Y - viewport.Top;
        PrintPosition(canvas, (viewPortX, viewPortY), boardColorMapper(board[position.X, position.Y], true));

        ctx.Refresh();

        return viewport;
    }

    private static (int Top, int Left) DetermineCanvasViewport(Canvas canvas, (int X, int Y) position)
    {
        var left = Convert.ToInt32(Math.Floor((double)position.X / canvas.Width)) * canvas.Width;
        var top = Convert.ToInt32(Math.Floor((double)position.Y / canvas.Height)) * canvas.Height;

        return (top, left);
    }

    public static void RenderBoard(Canvas canvas, char[,] board, (int Top, int Left) newViewport, Func<char, bool, Color> colorMapper)
    {
        for (var y = 0; y < canvas.Height; y++)
        {
            var boardY = y + newViewport.Top;
            for (var x = 0; x < canvas.Width; x++)
            {
                var boardX = x + newViewport.Left;

                if(boardX < board.GetLength(1) && boardY < board.GetLength(0))
                {
                    var symbol = board[boardY, boardX];
                    PrintPosition(canvas, (x, y), colorMapper(symbol, false));
                }

                if(boardX >= board.GetLength(1))
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

    private static void PrintPosition(Canvas canvas, (int X, int Y) position, Color color)
    {
        if(position.X < 0 || position.Y < 0 || position.X >= canvas.Width || position.Y >= canvas.Height)
        {
            return;
        }

        canvas.SetPixel(position.X, position.Y, color);
    }
}