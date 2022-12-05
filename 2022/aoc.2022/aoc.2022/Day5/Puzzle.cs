using System;
using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day5;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    public string DoPuzzle(string input)
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public void Part1Dev()
    {
        var result = DoPuzzle("Aoc.Day5.input.dev.txt");
        Assert.Equal("CMZ", result);
    }

    [Fact]
    public void Part1()
    {
        var result = DoPuzzle("Aoc.Day5.input.txt");
        Assert.Equal("CMZ", result);
        _output.WriteLine($"Contained pairs: {result}");
    }

    [Fact]
    public void Part2Dev()
    {
        var result = DoPuzzle("Aoc.Day5.input.dev.txt");
        Assert.Equal("CMZ", result);
    }

    [Fact]
    public void Part2()
    {
        var result = DoPuzzle("Aoc.Day5.input.txt");
        Assert.Equal("CMZ", result);
        _output.WriteLine($"Contained pairs: {result}");
    }
}