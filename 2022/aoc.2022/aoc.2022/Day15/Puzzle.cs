using System;
using System.Collections.Generic;
using System.Linq;

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

        var (min, max) = (long.MaxValue, long.MinValue);

        foreach (var sensor in sensors)
        {
            var slice = sensor.SensorAreadSlice(row);
            if(slice != null)
            {
                var (sliceMin, sliceMax) = slice.Value;
                min = Math.Min(sliceMin, min);
                max = Math.Max(sliceMax, max);
            }
            
        }

        var beaconsOnRow = beacons.Count(b => b.Y == row);
        return max - min + 1 - beaconsOnRow;
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