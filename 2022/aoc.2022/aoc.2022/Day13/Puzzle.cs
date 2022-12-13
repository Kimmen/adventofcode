using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Aoc.Day13;

public partial class Puzzle
{
    private static int CalculatePackagesInRightOrder(string input)
    {
        var packetPairs = InputReader
            .ReadLinesFromResource(input)
            .ChunkBy(string.IsNullOrWhiteSpace)
            .Select(linePairs =>
            {
                return (ArrayPacket.Parse(linePairs.First()), ArrayPacket.Parse(linePairs.Last()));
            })
            .Select(x =>
            {
                var (first, second) = x;
                return first.CompareTo(second) switch
                {
                    -1 => true,
                    1 => false,
                    _ => throw new InvalidOperationException()
                };
            })
            .ToList();

        return packetPairs
            .Select((x, i) => (Index: i + 1, IsInOrder: x))
            .Where(x => x.IsInOrder)
            .Sum(x => x.Index);
    }

    private static int CalculateDecoderKey(string input)
    {
        var packetPairs = InputReader
            .ReadLinesFromResource(input)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Concat(new[] { "[[2]]", "[[6]]" })
            .Select(ArrayPacket.Parse)
            .Order(new ArrayPacketComparer())
            .ToList();

        var divider2Index = packetPairs.FindIndex(x => x.InputLine == "[[2]]") + 1;
        var divider6Index = packetPairs.FindIndex(x => x.InputLine == "[[6]]") + 1;

        return divider2Index * divider6Index;
    }

    [Fact]
    public void Part1Dev()
    {
        var result = CalculatePackagesInRightOrder("Aoc.Day13.input.dev.txt");
        Assert.Equal(13, result);
    }

    [Fact]
    public void Part1()
    {
        var result = CalculatePackagesInRightOrder("Aoc.Day13.input.txt");
        Assert.Equal(6420, result);
    }

    [Fact]
    public void Part2Dev()
    {
        var result = CalculateDecoderKey("Aoc.Day13.input.dev.txt");
        Assert.Equal(140, result);
    }

    [Fact]
    public void Part2()
    {
        var result = CalculateDecoderKey("Aoc.Day13.input.txt");
        Assert.Equal(22000, result);
    }
}