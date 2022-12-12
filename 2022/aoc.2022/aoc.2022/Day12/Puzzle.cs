using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;

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

    private int DetermineShortestHike(string input, Func<(Terrain terrain, Elevation start), Elevation[]> determineStartingPoints)
    {
        var data = InputReader.ReadContentFromResource(input);
        var (terrain, start, end) = Terrain.Parse(data);

        var startingPoints = determineStartingPoints((terrain, start));
        var minSteps = int.MaxValue;

        foreach (var startingPoint in startingPoints)
        {
            var shortestPath = AStar.DetermineShortestPath(terrain, start, end);
            minSteps = Math.Min(minSteps, shortestPath.Count() - 1);
        }


        return minSteps;
    }

    private Elevation[] HikeOnlyFromStart((Terrain terrain, Elevation start) x)
    {
        return new[] { x.start }; 
    }

    [Fact]
    public void Part1Dev()
    {
        var shortestPath = DetermineShortestHike("Aoc.Day12.input.dev.txt", HikeOnlyFromStart);
        Assert.Equal(31, shortestPath);
    }

    [Fact]
    public void Part1()
    {
        var shortestPath = DetermineShortestHike("Aoc.Day12.input.txt", HikeOnlyFromStart);
        Assert.Equal(391, shortestPath);
    }
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