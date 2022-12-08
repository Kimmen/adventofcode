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
    
    private int GetTotalSizeOfDirectoriesExceeding(string input, Func<Directory, int> calculateDirectories)
    {
        var root = DirectoryBuilder.Build(InputReader.ReadLinesFromResource(input));
        return calculateDirectories(root);
    }
    
    private static int CalculateTotalSizeOfMinimalDirs(Directory root)
    {
        var directories = root.FlattenDirectories();
        return directories
            .Select(x => x.Size)
            .Where(x => x <= 100000)
            .Sum();
    }
    
    private static int CalculateClosestDeletableDirectory(Directory root)
    {
        int unusedSpace = 70_000_000 - root.Size;
        
        var firstDirSizeForUpgrade = root.FlattenDirectories()
            .Select(x => x.Size)
            .Order()
            .SkipWhile(x => unusedSpace + x <= 30000000)
            .FirstOrDefault();

        return firstDirSizeForUpgrade;
    }

    [Fact]
    public void Part1Dev()
    {
        var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.dev.txt", CalculateTotalSizeOfMinimalDirs);
        Assert.Equal(95437, result);
    }
    
    [Fact]
    public void Part1()
    {
        var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.txt", CalculateTotalSizeOfMinimalDirs);
        Assert.Equal(1325919, result);
        _output.WriteLine($"Size: {result}");
    }
    
    [Fact]
    public void Part2Dev()
    {
        var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.dev.txt", CalculateClosestDeletableDirectory);
        Assert.Equal(24933642, result);
    }
    
    [Fact]
    public void Part2()
    {
        var result = GetTotalSizeOfDirectoriesExceeding("Aoc.Day7.input.txt", CalculateClosestDeletableDirectory);
        Assert.Equal(2050735, result);
        _output.WriteLine($"Top: {result}");
    }
}
