using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime;

using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day9;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }
    
    private record struct Knot(int x, int y);

    private int GetTailPositionCount(string input, int ropeLength)
    {
        var headActions = InputReader
            .ReadLinesFromResource(input)
            .ToArray();

        var visited = new HashSet<Knot>();
        var rope = Enumerable.Repeat(new Knot(0, 0), ropeLength).ToList();

        foreach (var action in headActions)
        {
            var direction = action[0];
            var steps = int.Parse(action[2..]);

            for (var i = 0; i < steps; i++)
            {
                rope[0] = Move(rope[0], direction);

                for (int j = 1; j < rope.Count; j++)
                {
                    rope[j] = MoveKnot(rope[j], rope[j - 1]);
                }

                visited.Add(rope.Last());
            }
        }

        return visited.Count;
    }

    private Knot Move(Knot pos, char direction)
    {
        return direction switch
        {
            'R' => pos with { x = pos.x + 1 },
            'L' => pos with { x = pos.x - 1 },
            'U' => pos with { y = pos.y + 1 },
            'D' => pos with { y = pos.y - 1 },
            _ => throw new InvalidOperationException()
        };
    }

    private Knot MoveKnot(Knot current, Knot ahead)
    {
        var (dx, dy) = (ahead.x - current.x, ahead.y - current.y);

        if(Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
        {
            return current with
            {
                x = current.x += Math.Sign(dx),
                y = current.y += Math.Sign(dy)
            };

        }

        return current;
    }

    [Fact]
    public void Part1Dev()
    {
        var result = GetTailPositionCount("Aoc.Day9.input.dev.txt", 2);
        Assert.Equal(13, result);
    }

    [Fact]
    public void Part1()
    {
        var result = GetTailPositionCount("Aoc.Day9.input.txt", 2);
        Assert.Equal(6018, result);
    }
    //
    // [Fact]
    // public void Part2Dev()
    // {
    //     var result = GetHighestScenicScore("Aoc.Day8.input.dev.txt");
    //     Assert.Equal(8, result);
    // }
    //
    // [Fact]
    // public void Part2()
    // {
    //     var result = GetHighestScenicScore("Aoc.Day8.input.txt");
    //     Assert.Equal(301392, result);
    //     _output.WriteLine($"{result}");
    // }
}