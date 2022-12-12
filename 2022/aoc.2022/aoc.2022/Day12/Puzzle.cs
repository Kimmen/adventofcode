using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day12;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private int DetermineShortestPath(string input)
    {
        var data = InputReader.ReadContentFromResource(input);
        var (terrain, start, end) = Terrain.Parse(data);

        var shortestPath = AStar.DetermineShortestPath(terrain, start, end);

        return shortestPath.Count();
    }

    [Fact]
    public void Part1Dev()
    {
        var shortestPath = DetermineShortestPath("Aoc.Day12.input.dev.txt");
        Assert.Equal(31, shortestPath);
    }
    //
    // [Fact]
    // public void Part1()
    // {
    //     var monkeyBusiness = DetermineMonkeyBusiness("Aoc.Day11.input.txt", 20, GoodMonkeyReleifer);
    //     Assert.Equal(54054L, monkeyBusiness);
    // }
    //
    // [Fact]
    // public void Part2Dev()
    // {
    //     var monkeyBusiness = DetermineMonkeyBusiness("Aoc.Day11.input.dev.txt", 10000, ModulusArchmeticDevReleifer);
    //     Assert.Equal(2713310158L, monkeyBusiness);
    // }
    //
    // [Fact]
    // public void Part2()
    // {
    //     var monkeyBusiness = DetermineMonkeyBusiness("Aoc.Day11.input.txt", 10000, ModulusArchmeticReleifer);
    //     Assert.Equal(14314925001L, monkeyBusiness);
    // }
}