using System;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day2;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private static long CalculateTotalScore(string inputName, int includeTopCount)
    {
        var totalScore = Helpers.ReadLinesFromResource(inputName)
            .Select(x => x.Split(' '))
            .Select(x => (Opponent: GetShapeFromOpponent(x[0]), Your: GetShapeFromYour(x[1])))
            .Select(x => CalculateShapeScore(x.Your) + CalculateRoundScore(x.Opponent, x.Your))
            .Sum();

        return totalScore;
    }

    private static int CalculateRoundScore(Shape opponent, Shape your)
    {
        return (opponent, your) switch
        {
            (Shape.Rock, Shape.Scissor) => 0,
            (Shape.Paper, Shape.Rock) => 0,
            (Shape.Scissor, Shape.Paper) => 0,
            (Shape.Scissor, Shape.Rock) => 6,
            (Shape.Rock, Shape.Paper) => 6,
            (Shape.Paper, Shape.Scissor) => 6,
            _ => 3
        };
    }

    private static int CalculateShapeScore(Shape shape)
    {
        return shape switch
        {
            Shape.Rock => 1,
            Shape.Paper => 2,
            Shape.Scissor => 3,
            _ => throw new NotImplementedException(),
        };
    }

    private static Shape GetShapeFromYour(string yourChoice)
    {
        return yourChoice switch
        {
            "X" => Shape.Rock,
            "Y" => Shape.Paper,
            "Z" => Shape.Scissor,
            _ => throw new NotImplementedException()
        };
    }

    private static Shape GetShapeFromOpponent(string opponentChoice)
    {
        return opponentChoice switch
        {
            "A" => Shape.Rock,
            "B" => Shape.Paper,
            "C" => Shape.Scissor,
            _ => throw new NotImplementedException()
        };
    }

    [Fact]
    public void Part1Dev()
    {
        var totalScore = CalculateTotalScore("Aoc.Day2.input.dev.txt", 1);
        Assert.StrictEqual(15, totalScore);
    }

    [Fact]
    public void Part1()
    {
        var totalScore = CalculateTotalScore("Aoc.Day2.input.txt", 1);
        _output.WriteLine($"Total score: {totalScore}");
        Assert.StrictEqual(12586, totalScore);
    }
}