using System;
using System.Collections.Generic;

using Xunit;

namespace Aoc.Day14;

using Contains = System.Func<(int x, int y), HashSet<(int x, int y)>, (int x, int y), (int x, int y), bool>;
using ValidPos = System.Func<(int x, int y), HashSet<(int x, int y)>, (int x, int y), (int x, int y), bool>;

public class Puzzle
{
    private int DetermineUnitOfSands(string input, Contains contains, ValidPos validPos)
    {
        var addedSand = 0;
        var (plottedRockFormation, min, max) = RockFormation.Plot(input);

        bool ContainsPredicate((int x, int y) p)
        {
           return contains(p, plottedRockFormation!, min, max);
        } 

        bool ValidPosPredicate((int x, int y) p)
        {
            return validPos(p, plottedRockFormation, min, max);
        }

        (int x, int y)? sandPosition;
        do
        {
            sandPosition = MoveSand((500, 0), ContainsPredicate, ValidPosPredicate);
            if(sandPosition.HasValue)
            {
                plottedRockFormation.Add(sandPosition.Value);
                addedSand++;
            }
            
        } while (sandPosition.HasValue);
        
        return addedSand;
    }

    private static (int x, int y)? MoveSand((int x, int y) s, Predicate<(int x, int y)> contains, Predicate<(int x, int y)> validPos)
    {
        while (validPos(s))
        {
            (int x, int y) next;
            if (contains(next = (s.x, s.y + 1)) &&
                contains(next = (s.x - 1, s.y + 1)) &&
                contains(next = (s.x + 1, s.y + 1)))
            {
                return s;
            }

            s = next;
        }

        return null;
    }

    private bool IsPlotted((int x, int y) p, HashSet<(int x, int y)> plotted, (int x, int y) min, (int x, int y) max)
    {
        return plotted.Contains(p);
    }

    private bool InsideBounderies((int x, int y) p, HashSet<(int x, int y)> plotted, (int x, int y) min, (int x, int y) max)
    {
        if (p.x < min.x || p.x > max.x) { return false; }
        if (p.y < min.y || p.y > max.y) { return false; }

        return true;
    }

    private bool IsPlottedOrHitRockBottom((int x, int y) p, HashSet<(int x, int y)> plotted, (int x, int y) min, (int x, int y) max)
    {
        return plotted.Contains(p) || p.y > max.y + 1;
    }

    private bool IsNonPlottedStart((int x, int y) p, HashSet<(int x, int y)> plotted, (int x, int y) min, (int x, int y) max)
    {
        return p != (500, 0) || !plotted.Contains(p);
    }

    [Fact]
    public void Part1Dev()
    {
        var result = DetermineUnitOfSands("Aoc.Day14.input.dev.txt", IsPlotted, InsideBounderies);
        Assert.Equal(24, result);
    }

    [Fact]
    public void Part1()
    {
        var result = DetermineUnitOfSands("Aoc.Day14.input.txt", IsPlotted, InsideBounderies);
        Assert.Equal(793, result);
    }

    [Fact]
    public void Part2Dev()
    {
        var result = DetermineUnitOfSands("Aoc.Day14.input.dev.txt", IsPlottedOrHitRockBottom, IsNonPlottedStart);
        Assert.Equal(93, result);
    }

    [Fact]
    public void Part2()
    {
        var result = DetermineUnitOfSands("Aoc.Day14.input.txt", IsPlottedOrHitRockBottom, IsNonPlottedStart);
        Assert.Equal(24166, result);
    }
}