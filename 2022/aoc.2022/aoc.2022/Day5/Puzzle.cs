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

    public string GetTopFromEachStack(string input)
    {
        var rows = Helpers.ReadLinesFromResource(input).ToList();
        var stacks = ReadStacks(rows);
        var instructions = ReadInstructions(rows);

        foreach ( var instruction in instructions) 
        {
            stacks.Move(instruction);
        }

        return stacks.PeekAll();
    }

    private CrateStackCollection ReadStacks(List<string> rows)
    {
        return CrateStackCollection.Parse(rows);
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
        var result = GetTopFromEachStack("Aoc.Day5.input.dev.txt");
        Assert.Equal("CMZ", result);
    }

    [Fact]
    public void Part1()
    {
        var result = GetTopFromEachStack("Aoc.Day5.input.txt");
        Assert.Equal("TBVFVDZPN", result);
        _output.WriteLine($"Top: {result}");
    }

    [Fact]
    public void Part2Dev()
    {
        var result = GetTopFromEachStack("Aoc.Day5.input.dev.txt");
        Assert.Equal("CMZ", result);
    }

    [Fact]
    public void Part2()
    {
        var result = GetTopFromEachStack("Aoc.Day5.input.txt");
        Assert.Equal("CMZ", result);
        _output.WriteLine($"Contained pairs: {result}");
    }
}