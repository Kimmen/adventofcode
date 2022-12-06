using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day6;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private int GetFirstMarkerIndex(string dataStream, int windowSize)
    {
        var data = dataStream.AsSpan();

        for (int i = 0; i < data.Length; i++)
        {
            var marker = data.Slice(i, windowSize);
            
            if(!ContainsDuplicates(marker))
            {
                return i + windowSize;
            }
        }

        return -1;
    }

    private bool ContainsDuplicates(ReadOnlySpan<char> marker)
    {
        var set = new HashSet<char>();
        foreach (char c in marker)
        {
            if (set.Contains(c)) return true;
            set.Add(c);
        }

        return false;
    }

    [Fact]
    public void Part1Dev()
    {
        var index1 = GetFirstMarkerIndex("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4);
        var index2 = GetFirstMarkerIndex("bvwbjplbgvbhsrlpgdmjqwftvncz", 4);
        var index3 = GetFirstMarkerIndex("nppdvjthqldpwncqszvftbrmjlhg", 4);
        var index4 = GetFirstMarkerIndex("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4);
        var index5 = GetFirstMarkerIndex("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4);

        Assert.Equal(7, index1);
        Assert.Equal(5, index2);
        Assert.Equal(6, index3);
        Assert.Equal(10, index4);
        Assert.Equal(11, index5);
    }

    [Fact]
    public void Part1()
    {
        var result = GetFirstMarkerIndex(InputReader.ReadContentFromResource("Aoc.Day6.input.txt"), 4);
        _output.WriteLine($"Marker index: {result}");
        Assert.Equal(1833, result);
    }

    [Fact]
    public void Part2Dev()
    {
        var index1 = GetFirstMarkerIndex("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 14);
        var index2 = GetFirstMarkerIndex("bvwbjplbgvbhsrlpgdmjqwftvncz", 14);
        var index3 = GetFirstMarkerIndex("nppdvjthqldpwncqszvftbrmjlhg", 14);
        var index4 = GetFirstMarkerIndex("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 14);
        var index5 = GetFirstMarkerIndex("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 14);

        Assert.Equal(19, index1);
        Assert.Equal(23, index2);
        Assert.Equal(23, index3);
        Assert.Equal(29, index4);
        Assert.Equal(26, index5);
    }

    [Fact]
    public void Part2()
    {
        var result = GetFirstMarkerIndex(InputReader.ReadContentFromResource("Aoc.Day6.input.txt"), 14);
        _output.WriteLine($"Marker index: {result}");
        Assert.Equal(3425, result);
    }
}
