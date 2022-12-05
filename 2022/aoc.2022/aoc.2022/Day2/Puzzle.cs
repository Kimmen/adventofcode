using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day2;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    private static readonly IDictionary<Shape, Shape> _winsOver = new Dictionary<Shape, Shape>
    {
        [Shape.Rock] = Shape.Scissor,
        [Shape.Paper] = Shape.Rock,
        [Shape.Scissor] = Shape.Paper,
    };

    private static readonly IDictionary<Shape, Shape> _loseTo = _winsOver.ToDictionary(x => x.Value, x => x.Key);

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private static int CalculateTotalScore(string inputName, Func<Shape, string, Shape> yourShapeEvaluator)
    {
        var totalScore = InputReader.ReadLinesFromResource(inputName)
            .Select(x => x.Split(' '))
            .Select(x => (Opponent: GetShapeFromOpponent(x[0]), Your: yourShapeEvaluator(GetShapeFromOpponent(x[0]), x[1])))
            .Select(x => CalculateShapeScore(x.Your) + CalculateRoundScore(x.Opponent, x.Your))
            .Sum();

        return totalScore;
    }

    private static int CalculateRoundScore(Shape opponent, Shape your)
    {
        if (_winsOver[opponent] == your) return 0; 
        if (_loseTo[opponent] == your) return 6; 
        else return 3;
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

    private static Shape GetShapeFromOpponent(string opponent)
    {
        return opponent switch
        {
            "A" => Shape.Rock,
            "B" => Shape.Paper,
            "C" => Shape.Scissor,
            _ => throw new NotImplementedException()
        };
    }

    private static Shape StraightYourShapeEvalutor(Shape _, string your)
    {
        return your switch
        {
            "X" => Shape.Rock,
            "Y" => Shape.Paper,
            "Z" => Shape.Scissor,
            _ => throw new NotImplementedException()
        };
    }

    private static Shape OutcomeYourShapeEvalutor(Shape opponent, string outcome)
    {
        return outcome switch
        {
            "X" => _winsOver[opponent],
            "Y" => opponent,
            "Z" => _loseTo[opponent],
            _ => throw new NotImplementedException()
        };
    
    }

    [Fact]
    public void Part1Dev()
    {
        var totalScore = CalculateTotalScore("Aoc.Day2.input.dev.txt", StraightYourShapeEvalutor);
        Assert.StrictEqual(15, totalScore);
    }

    [Fact]
    public void Part1()
    {
        var totalScore = CalculateTotalScore("Aoc.Day2.input.txt", StraightYourShapeEvalutor);
        _output.WriteLine($"Total score: {totalScore}");
        Assert.StrictEqual(12586, totalScore);
    }

    [Fact]
    public void Part2Dev()
    {
        var totalScore = CalculateTotalScore("Aoc.Day2.input.dev.txt", OutcomeYourShapeEvalutor);
        Assert.StrictEqual(12, totalScore);
    }

    [Fact]
    public void Part2()
    {
        var totalScore = CalculateTotalScore("Aoc.Day2.input.txt", OutcomeYourShapeEvalutor);
        _output.WriteLine($"Total score: {totalScore}");
        Assert.StrictEqual(13193, totalScore);
    }
}