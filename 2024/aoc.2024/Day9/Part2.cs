using System.Collections.Concurrent;
using System.Net;
using Spectre.Console;

namespace Aoc.Day9;

public class Part2 : IChallenge
{
    private string _inputExt = "txt";

    private long? _expectedResult = null; //6440697749191
                                          //6440697749191
                                          //6439319377569
                                          //8657824158422
                                          //15986296176501
                                          //6439319377569

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
        var spaces = Enumerable.Range(0, 10).Select(_ => new SortedSet<int>()).ToList();

        var index = 0;
        foreach (var c in chars)
        {
            var blockSize = CharToNumber(c);
            try
            {
                if (isFile)
                {
                    fileBlocks.Add((fileId, blockSize, index));
                    fileId++;
                }
                else
                {
                    spaces[blockSize].Add(index);
                }
            }
            finally
            {
                isFile = !isFile;
                index += blockSize;
            }
        }


        fileBlocks.Sort((a, b) => b.FileId.CompareTo(a.FileId));

        for (var i = 0; i < fileBlocks.Count; i++)
        {
            var (fId, fileSize, fileIndex) = fileBlocks[i];
            (int SpaceSize, int SpaceIndex) space = default;

            for (var j = fileSize; j < spaces.Count; j++)
            {
                var si = spaces[j].FirstOrDefault(x => x < fileIndex);
                if (si == default)
                {
                    continue;
                }

                spaces[j].Remove(si);
                space = (j, si);
                break;
            }

            if (space == default)
            {
                continue;
            }

            var (spaceSize, spaceIndex) = space;
            fileBlocks[i] = (fId, fileSize, spaceIndex);

            if (spaceSize > fileSize)
            {
                var remainingSpace = spaceSize - fileSize;
                var remainingSpaceIndex = spaceIndex + fileSize;
                spaces[remainingSpace].Add(remainingSpaceIndex);
            }
        }

        fileBlocks.Sort((a, b) => a.StartIndex.CompareTo(b.StartIndex));
        var checksum = 0L;
        foreach (var fileBlock in fileBlocks)
        {
            var (fId, size, blockIndex) = fileBlock;
            for (var i = 0; i < size; i++)
            {
                if (blockIndex == 45983)
                {

                }
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