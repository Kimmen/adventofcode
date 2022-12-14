using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

using Xunit;

using static System.Net.Mime.MediaTypeNames;

namespace Aoc.Day14;

public class Puzzle
{
    private int DetermineUnitOfSands(string input)
    {
        var addedSand = 0;
        var (plottedRockFormation, min, max) = RockFormation.Plot(input);

        (int x, int y)? sandPosition;
        do
        {
            sandPosition = MoveSand((500, 0), plottedRockFormation, min, max);
            if(sandPosition.HasValue)
            {
                plottedRockFormation.Add(sandPosition.Value);
                addedSand++;
            }
            
        } while (sandPosition.HasValue);
        
        return addedSand;
    }

    private bool InsideBounderies((int x, int y) p, (int x, int y) min, (int x, int y) max)
    {
        if(p.x < min.x || p.x > max.x) { return false; }
        if(p.y < min.y || p.y > max.y) { return false; } 
        
        return true;
    }

    private (int x, int y)? MoveSand((int x, int y) s, HashSet<(int x, int y)> occupied, (int x, int y) min, (int x, int y) max)
    {
        while (InsideBounderies(s, min, max))
        {
            (int x, int y) next;
            if (!occupied.Contains(next = (s.x, s.y + 1)) ||
                !occupied.Contains(next = (s.x - 1, s.y + 1)) ||
                !occupied.Contains(next = (s.x + 1, s.y + 1)))
            {
                s = next;
                continue;
            }

            return s;
        }

        return null;
    }

    [Fact]
    public void Part1Dev()
    {
        var result = DetermineUnitOfSands("Aoc.Day14.input.dev.txt");
        Assert.Equal(24, result);
    }

    [Fact]
    public void Part1()
    {
        var result = DetermineUnitOfSands("Aoc.Day14.input.txt");
        Assert.Equal(793, result);
    }

    //[Fact]
    //public void Part2Dev()
    //{
    //    var result = CalculateDecoderKey("Aoc.Day13.input.dev.txt");
    //    Assert.Equal(140, result);
    //}

    //[Fact]
    //public void Part2()
    //{
    //    var result = CalculateDecoderKey("Aoc.Day13.input.txt");
    //    Assert.Equal(22000, result);
    //}
}