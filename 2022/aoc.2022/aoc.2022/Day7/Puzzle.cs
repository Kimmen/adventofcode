using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day7;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }
    
    private int GetTotalSizeOfDirectoriesExceeding(string input, int limitSize)
    {
        var directory = DirectoryBuilder.Build(InputReader.ReadLinesFromResource(input));
        var totalSize =  MatchDirectories(directory, x => x.Size <= limitSize)
                .Select(x => x.Size)
                .Sum();

        return totalSize;
    }

    private IEnumerable<Directory> MatchDirectories(Directory directory, Func<Directory, bool> match)
    {
        if (match(directory))
        {
            yield return directory;
        }

        foreach (var child in directory.Children)
        {
            if (child is not Directory d)
            {
                continue;
            }
            
            var matchingChildren = MatchDirectories(d, match);

            foreach (var matchingChild in matchingChildren)
            {
                yield return matchingChild;
            }
        }
    }

    [Fact]
    public void Part1Dev()
    {
        var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.dev.txt", 100000);
        Assert.Equal(95437, result);
    }
    
    [Fact]
    public void Part1()
    {
        var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.txt", 100000);
        Assert.Equal(1325919, result);
        _output.WriteLine($"Size: {result}");
    }
    //
    // [Fact]
    // public void Part2Dev()
    // {
    //     var result = GetTopFromEachStack("Aoc.Day5.input.dev.txt", new AllAtOnceCrateMover());
    //     Assert.Equal("MCD", result);
    // }
    //
    // [Fact]
    // public void Part2()
    // {
    //     var result = GetTopFromEachStack("Aoc.Day5.input.txt", new AllAtOnceCrateMover());
    //     Assert.Equal("VLCWHTDSZ", result);
    //     _output.WriteLine($"Top: {result}");
    // }
}
