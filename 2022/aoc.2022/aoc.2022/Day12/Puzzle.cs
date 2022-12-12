using System;
using System.Linq;

using Xunit;

namespace Aoc.Day12;

public class Puzzle
{
    private int DetermineShortestHike(string input, Func<(Terrain terrain, Elevation start), Elevation[]> determineStartingPoints)
    {
        var data = InputReader.ReadContentFromResource(input);
        var (terrain, start, end) = Terrain.Parse(data);

        var startingPoints = determineStartingPoints((terrain, start));
        var minSteps = int.MaxValue;

        var shortestPath = AStar.DetermineShortestPath(terrain, startingPoints, end);
        minSteps = Math.Min(minSteps, shortestPath.Count() - 1);

        return minSteps;
    }

    private Elevation[] HikeOnlyFromStart((Terrain terrain, Elevation start) x)
    {
        return new[] { x.start }; 
    }

    private Elevation[] HikeFromAllLowest((Terrain terrain, Elevation start) x)
    {
        var (terrain, _) = x;

        return terrain.GetAllElevations.Where(x => x.Value == 0).ToArray();
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

    [Fact]
    public void Part2Dev()
    {
        var shortestPath = DetermineShortestHike("Aoc.Day12.input.dev.txt", HikeFromAllLowest);
        Assert.Equal(29, shortestPath);
    }

    [Fact]
    public void Part2()
    {
        var shortestPath = DetermineShortestHike("Aoc.Day12.input.txt", HikeFromAllLowest);
        Assert.Equal(386, shortestPath);
    }
}