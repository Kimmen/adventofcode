using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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

    public string GetTopFromEachStack(string input, ICrateMover crateMover)
    {
        var rows = InputReader.ReadLinesFromResource(input).ToList();
        var stacks = BuildCrateCollection(rows, crateMover);
        var instructions = ReadInstructions(rows);

        foreach (var instruction in instructions) 
        {
            stacks.Move(instruction);
        }

        return stacks.PeekAll();
    }

    private CrateCollection BuildCrateCollection(List<string> rows, ICrateMover crateMover)
    {
        return CrateCollection.Parse(rows, crateMover);
    }

    private IEnumerable<MoveInstruction> ReadInstructions(List<string> rows)
    {
        var instructionsInput = rows
            .SkipWhile(x => !string.IsNullOrWhiteSpace(x))
            .Skip(1); //remove empty line

        return instructionsInput
            .Select(x => MoveInstruction.Parse(x))
            .ToImmutableList();
    }

    [Fact]
    public void Part1Dev()
    {
        var result = GetTopFromEachStack("Aoc.Day5.input.dev.txt", new OneAtATimeCrateMover());
        Assert.Equal("CMZ", result);
    }

    [Fact]
    public void Part1()
    {
        var result = GetTopFromEachStack("Aoc.Day5.input.txt", new OneAtATimeCrateMover());
        Assert.Equal("TBVFVDZPN", result);
        _output.WriteLine($"Top: {result}");
    }

    [Fact]
    public void Part2Dev()
    {
        var result = GetTopFromEachStack("Aoc.Day5.input.dev.txt", new AllAtOnceCrateMover());
        Assert.Equal("MCD", result);
    }

    [Fact]
    public void Part2()
    {
        var result = GetTopFromEachStack("Aoc.Day5.input.txt", new AllAtOnceCrateMover());
        Assert.Equal("VLCWHTDSZ", result);
        _output.WriteLine($"Top: {result}");
    }
}
