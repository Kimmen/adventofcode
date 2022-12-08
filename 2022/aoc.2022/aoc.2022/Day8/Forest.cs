using System;
using System.Linq;

namespace Aoc.Day8;

internal class Forest
{
    public static (int[][] Trees, int Height, int Width) Build(string input)
    {
        var trees = InputReader
                    .ReadLinesFromResource(input)
                    .Select(x => x.Select(c => c - '0').ToArray())
                    .ToArray();

        return (trees, trees.Length, trees.First().Length);
    }

    public static void Traverse(int height, int width, Action<(int h, int w)> visit)
    {
        for (int h = 1; h < height - 1; h++)
        {
            for (int w = 1; w < width - 1; w++)
            {
                visit((h, w));
            }
        }
    }

    public static bool IsInBoundery(int[][] trees, int h, int w)
    {
        return h >= 0 && w >= 0 && h < trees.Length && w < trees[0].Length;
    }
}
