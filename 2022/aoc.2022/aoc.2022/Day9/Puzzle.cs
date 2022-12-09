using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    
    private record struct Pos(int x, int y);

    private int GetTailPositionCount(string input)
    {
        var headActions = InputReader
            .ReadLinesFromResource(input)
            .ToArray();

        var tailPositions = new HashSet<Pos>(headActions.Length * 10);
        var headPos = new Pos(0, 0);
        var tailPos = new Pos(0, 0);
        foreach (var action in headActions)
        {
            var direction = action[0];
            var steps = action[2] - '0';

            for (var i = 0; i < steps; i++)
            {
                headPos = Move(headPos, direction);
                tailPos = MoveTail(tailPos, headPos, direction);

                tailPositions.Add(tailPos);
            }
        }

        return tailPositions.Count;
    }

    private Pos Move(Pos pos, char direction, bool reverse = false)
    {
        var reverseFactor = reverse ? -1 : 1;
        return direction switch
        {
            'R' => pos with { x = pos.x + reverseFactor },
            'L' => pos with { x = pos.x - reverseFactor },
            'U' => pos with { y = pos.y + reverseFactor },
            'D' => pos with { y = pos.y - reverseFactor },
            _ => throw new InvalidOperationException()
        };
    }

    private Pos MoveTail(Pos tail, Pos head, char direction)
    {
        // if (tail.y > head.y + 1)
        // {
        //     tail = tail with { y = tail.y - 1 };
        //     if(tail.x)
        // }
        
        var dx = head.x - tail.x;
        var dy = head.y - tail.y;
        var xDist = Math.Abs(dx);
        var yDist = Math.Abs(dy);
        
        if (xDist <= 1 && yDist <= 1)
        {
            return tail;
        }
        
        return Move(head, direction, reverse: true);
    }

    [Fact]
    public void Part1Dev()
    {
        var result = GetTailPositionCount("Aoc.Day9.input.dev.txt");
        Assert.Equal(13, result);
    }

    [Fact]
    public void Part1()
    {
        var result = GetTailPositionCount("Aoc.Day9.input.txt");
        Assert.Equal(13, result);
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