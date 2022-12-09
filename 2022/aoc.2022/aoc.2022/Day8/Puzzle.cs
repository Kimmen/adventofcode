using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day8;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private int GetNumberOfVisibleTrees(string input)
    {
        var (trees, height, width) = Forest.Build(input);

        //Count edge, as they are visible.
        var visibleCount = height * 2 + (width - 2) * 2;

        Forest.Traverse(height, width, (coord) =>
        {
            var (h, w) = coord;
            if (IsVisibleInDirection(trees, h, w, -1, 0)
                    || IsVisibleInDirection(trees, h, w, 1, 0)
                    || IsVisibleInDirection(trees, h, w, 0, -1)
                    || IsVisibleInDirection(trees, h, w, 0, 1))
            {
                visibleCount++;
            }
        });

        return visibleCount;
    }


    private bool IsVisibleInDirection(int[][] trees, int h, int w, int dh, int dw)
    {
        var compare = trees[h][w];
        h += dh;
        w += dw;

        while (Forest.IsInBoundery(trees, h, w))
        {
            var current = trees[h][w];
            if (compare <= current)
            {
                return false;
            }

            h += dh;
            w += dw;
        }

        return true;
    }

    private int GetHighestScenicScore(string input)
    {
        var (trees, height, width) = Forest.Build(input);

        var maxScenicScore = 0;

        Forest.Traverse(height, width, (coord) =>
        {
            var (h, w) = coord;
            var scenicScore = CalculateViewDistanceInDirection(trees, h, w, -1, 0)
                    * CalculateViewDistanceInDirection(trees, h, w, 1, 0)
                    * CalculateViewDistanceInDirection(trees, h, w, 0, -1)
                    * CalculateViewDistanceInDirection(trees, h, w, 0, 1);
            
            if(scenicScore > maxScenicScore)
            {
                maxScenicScore = scenicScore;
            }
        });

        return maxScenicScore;
    }

    private int CalculateViewDistanceInDirection(int[][] trees, int h, int w, int dh, int dw)
    {
        var compare = trees[h][w];
        h += dh;
        w += dw;

        var score = 0;

        while (Forest.IsInBoundery(trees, h, w))
        {
            score++;
            var current = trees[h][w];
            if (compare <= current)
            {
                break;
            }

            h += dh;
            w += dw;
        }

        return score;
    }

    [Fact]
    public void Part1Dev()
    {
        var result = GetNumberOfVisibleTrees("Aoc.Day8.input.dev.txt");
        Assert.Equal(21, result);
    }

    [Fact]
    public void Part1()
    {
        var result = GetNumberOfVisibleTrees("Aoc.Day8.input.txt");
        Assert.Equal(1711, result);
        _output.WriteLine($"{result}");
    }

    [Fact]
    public void Part2Dev()
    {
        var result = GetHighestScenicScore("Aoc.Day8.input.dev.txt");
        Assert.Equal(8, result);
    }

    [Fact]
    public void Part2()
    {
        var result = GetHighestScenicScore("Aoc.Day8.input.txt");
        Assert.Equal(301392, result);
        _output.WriteLine($"{result}");
    }
}