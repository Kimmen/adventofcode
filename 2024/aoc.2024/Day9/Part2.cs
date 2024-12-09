using System.Collections.Concurrent;
using System.Net;
using Spectre.Console;

namespace Aoc.Day9;

public class Part2 : IChallenge
{
    private string _inputExt = "txt";

    private long? _expectedResult = null; //6440697749191 //6440697749191 //8657824158422

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 2858;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var chars = InputReader.StreamChars(PuzzleInput);
        var fileId = 0;
        var isFile = true;

        var fileBlocks = new List<(int FileId, int Size, int StartIndex)>();
        var spaceBlocks = new SortedSet<(int Size, int StartIndex)>();

        var index = 0;
        foreach (var c in chars)
        {
            var blockSize = CharToNumber(c);
            try
            {
                if (blockSize == 0)
                {
                    continue;
                }

                if (isFile)
                {
                    fileBlocks.Add((fileId, blockSize, index));
                    fileId++;
                }
                else
                {
                    spaceBlocks.Add((blockSize, index));
                }
            }
            finally
            {
                isFile = !isFile;
                index += blockSize;
            }
        }

        // AnsiConsole.WriteLine();

        for (var i = fileBlocks.Count - 1; i >= 0; i--)
        {
            var (fId, size, blockIndex) = fileBlocks[i];

            var space = spaceBlocks.FirstOrDefault(block => block.Size >= size);

            if (space == default || blockIndex > space.StartIndex)
            {
                continue;
            }

            spaceBlocks.Remove(space);
            var (spaceSize, spaceIndex) = space;
            fileBlocks[i] = (fId, size, spaceIndex);

            var remainingSpace = spaceSize - size;
            if (remainingSpace > 0)
            {
                spaceBlocks.Add((remainingSpace, spaceIndex + size));
            }
        }

        var checksum = 0L;
        foreach (var fileBlock in fileBlocks)
        {
            var (fId, size, blockIndex) = fileBlock;
            for (var i = 0; i < size; i++)
            {
                checksum += (blockIndex + i) * fId;
            }
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Checksum: ");
        PrintResult(checksum);
    }

    private static int CharToNumber(char c)
    {
        return c - '0';
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