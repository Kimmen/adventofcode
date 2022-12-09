using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using static Aoc.LoopUtilities;

namespace Aoc.Day9;

public class Puzzle
{    
    private record struct Knot(int x, int y);

    private int GetTailPositionCount(string input, int ropeLength)
    {
        var headMovements = InputReader.ReadLinesFromResource(input);

        var rope = Enumerable
            .Repeat(new Knot(0, 0), ropeLength)
            .ToList();

        var visited = new HashSet<Knot>();
        foreach (var movement in headMovements)
        {
            var direction = movement[0];
            var steps = int.Parse(movement[2..]);

            Repeat(steps, () =>
            {
                rope[0] = MoveHead(rope[0], direction);

                for (int knotIndex = 1; knotIndex < rope.Count; knotIndex++)
                {
                    var current = rope[knotIndex];
                    var previous = rope[knotIndex - 1];
                    rope[knotIndex] = FollowKnot(current, previous);
                }

                visited.Add(rope.Last());
            });
        }

        return visited.Count;
    }

    private static Knot MoveHead(Knot pos, char direction)
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

    private static Knot FollowKnot(Knot current, Knot ahead)
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

    [Fact]
    public void Part2Dev()
    {
        var result = GetTailPositionCount("Aoc.Day9.input.dev.txt", 10);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Part22Dev()
    {
        var result = GetTailPositionCount("Aoc.Day9.input.dev2.txt", 10);
        Assert.Equal(36, result);
    }

    [Fact]
    public void Part2()
    {
        var result = GetTailPositionCount("Aoc.Day9.input.txt", 10);
        Assert.Equal(2619, result);
    }
}