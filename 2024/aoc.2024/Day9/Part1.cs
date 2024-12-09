using System.Collections.Concurrent;
using Spectre.Console;

namespace Aoc.Day9;

public class Part1 : IChallenge
{
    private string _inputExt = "txt";

    private long? _expectedResult = null;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 1928;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var chars = InputReader.StreamChars(PuzzleInput);
        uint fileId = 1;
        var isFile = true;
        var blocks = new List<uint>(10000);

        foreach (var c in chars)
        {
            var blockSize = CharToNumber(c);
            try
            {
                if(blockSize == 0)
                {
                    continue;
                }
                for (var i = 0; i < blockSize; i++)
                {
                    var block = isFile ? fileId : 0;
                    blocks.Add(block);
                    AnsiConsole.Markup(isFile ? $"{block - 1}" : "[grey85].[/]");
                }

                if(isFile)
                {
                    fileId++;
                }
            }
            finally
            {
                isFile = !isFile;
            }
        }

        var spaceIndex = FindNextSpace(0);
        var index = blocks.Count - 1;

        while (spaceIndex < index)
        {
            try
            {
                var block = blocks[index];
                if(block == 0)
                {
                    continue;
                }

                (blocks[index], blocks[spaceIndex]) = (blocks[spaceIndex], blocks[index]);

                // var digits = (int)Math.Floor(Math.Log10(block - 1)) + 1;
                //
                // AnsiConsole.Cursor.MoveLeft(blocks.Count - consoleIndex);
                // AnsiConsole.Markup($"[maroon]{block - 1}[/]");
                // AnsiConsole.Cursor.MoveLeft(consoleIndex - spaceIndex + 1);
                // AnsiConsole.Markup("[maroon].[/]");
                //
                // AnsiConsole.Cursor.MoveLeft(1);
                // AnsiConsole.Markup($"[maroon]{block - 1}[/]");
                // AnsiConsole.Cursor.MoveRight(blocks.Count - (spaceIndex + 1));
                // AnsiConsole.Cursor.MoveLeft(blocks.Count - consoleIndex);
                // AnsiConsole.Markup("[maroon].[/]");
                // AnsiConsole.Cursor.MoveRight(blocks.Count - consoleIndex - 1);

                spaceIndex = FindNextSpace(spaceIndex + 1);
                // Thread.Sleep(1);
            }
            finally
            {
                index--;
            }
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Unique anti nodes: ");
        PrintResult(blocks.Where(n => n > 0).Select((n, i) => (n - 1) * i).Sum());
        return;

        int FindNextSpace(int f) => blocks.IndexOf(0, f);
    }

    private static uint CharToNumber(char c)
    {
        return (uint)(c - '0');
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