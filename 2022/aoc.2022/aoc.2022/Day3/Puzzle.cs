using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day3;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

   
    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private long CalculateItemPrioritySum(string input)
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void Part1Dev()
    {
        var prioritySum = CalculateItemPrioritySum("Aoc.Day3.input.dev.txt");
        Assert.StrictEqual(157, prioritySum);
    }

    [Fact]
    public void Part1()
    {
        var prioritySum = CalculateItemPrioritySum("Aoc.Day3.input.txt");
        Assert.StrictEqual(157, prioritySum);
        _output.WriteLine($"Priority sum: {prioritySum}");
    }

    [Fact]
    public void Part2Dev()
    {
        var prioritySum = CalculateItemPrioritySum("Aoc.Day3.input.dev.txt");
        Assert.StrictEqual(157, prioritySum);
    }

    [Fact]
    public void Part2()
    {
        var prioritySum = CalculateItemPrioritySum("Aoc.Day3.input.txt");
        Assert.StrictEqual(157, prioritySum);
        _output.WriteLine($"Priority sum: {prioritySum}");
    }
}