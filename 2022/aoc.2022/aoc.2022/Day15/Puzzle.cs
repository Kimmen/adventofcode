using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security;

using Xunit;

namespace Aoc.Day15;

public class Puzzle
{
    private long DetermineInvalidBeaconPositions(string input, long row)
    {
        var sensors = InputReader
            .ReadLinesFromResource(input)
            .Select(Sensor.Parse)
            .Where(s => s.Intersects(row))
            .ToList();

        var beacons = sensors.Select(x => x.ClosestBeacon)
            .Distinct()
            .ToList();

        var beaconsOnRow = beacons.Count(b => b.Y == row);
        var slices = sensors
            .Select(s => s.SensorAreadSlice(row))
            .Where(s => s.HasValue)
            .Select(s => s!.Value)
            .OrderBy(s => s.min)
            .ToList();

        var i = 1;
        while(i < slices.Count)
        {
            var s1 = slices[i - 1];
            var s2 = slices[i];
            if(Overlapping(s1, s2))
            {
                slices[i - 1] = Combine(s1, s2);
                slices.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        var sum = slices.Sum(x => x.max - x.min);

        return sum + 1 - beaconsOnRow;
    }

    private (long min, long max) Combine((long min, long max) s1, (long min, long max) s2)
    {
        return (Math.Min(s1.min, s2.min), Math.Max(s1.max, s2.max));
    }

    private bool Overlapping((long min, long max) lower, (long min, long max) higher)
    {
        return lower.max >= higher.min;
    }

    [Fact]
    public void Part1Dev()
    {
        var result = DetermineInvalidBeaconPositions("Aoc.Day15.input.dev.txt", 10);
        Assert.Equal(26, result);
    }

    [Fact]
    public void Part1()
    {
        var result = DetermineInvalidBeaconPositions("Aoc.Day15.input.txt", 2000000);
        Assert.Equal(4961647, result);
    }

    //[Fact]
    //public void Part2Dev()
    //{
    //    var result = DetermineUnitOfSands("Aoc.Day14.input.dev.txt", IsPlottedOrHitRockBottom, IsNonPlottedStart);
    //    Assert.Equal(93, result);
    //}

    //[Fact]
    //public void Part2()
    //{
    //    var result = DetermineUnitOfSands("Aoc.Day14.input.txt", IsPlottedOrHitRockBottom, IsNonPlottedStart);
    //    Assert.Equal(24166, result);
    //}
}