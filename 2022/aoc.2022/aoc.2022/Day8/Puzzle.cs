using System.ComponentModel.Design;
using System.Linq;
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
        var lines = InputReader.ReadLinesFromResource(input).ToArray();
        var size = (Height: lines.Length, Width: lines.First().Length);
        var treesVisibility = new  bool[size.Height, size.Width];
        
        var trees = InputReader
            .ReadLinesFromResource(input)
            .Select(x => x.Select(c => c - 'c').ToArray())//Convert to char to ints
            .ToArray();

        foreach (var horizontalTreeLine in trees)
        {
            
        }


    }

    [Fact]
    public void Part1Dev()
    {
        var result = GetNumberOfVisibleTrees("Aoc.Day8.input.dev.txt");
        Assert.Equal(21, result);
    }

    // [Fact]
    // public void Part1()
    // {
    //     var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.txt", CalculateTotalSizeOfMinimalDirs);
    //     Assert.Equal(1325919, result);
    //     _output.WriteLine($"Size: {result}");
    // }
    //
    // [Fact]
    // public void Part2Dev()
    // {
    //     var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.dev.txt", CalculateClosestDeletableDirectory);
    //     Assert.Equal(24933642, result);
    // }
    //
    // [Fact]
    // public void Part2()
    // {
    //     var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.txt", CalculateClosestDeletableDirectory);
    //     Assert.Equal(2050735, result);
    //     _output.WriteLine($"Top: {result}");
    // }
}
