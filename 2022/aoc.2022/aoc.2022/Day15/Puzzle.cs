using System;
using System.Collections;
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
            .ToList();

        var beacons = sensors.Select(x => x.ClosestBeacon)
            .Distinct()
            .ToList();

        var beaconsOnRow = beacons.Count(b => b.Y == row);
        var slices = GetAreaSlicesForRow(sensors, row);

        var sum = slices.Sum(x => x.max - x.min);

        return sum + 1 - beaconsOnRow;
    }

    public long DetermineTuningFrequency(string input, long minCoordinate, long maxCoordinate)
    {
        var sensors = InputReader
           .ReadLinesFromResource(input)
           .Select(Sensor.Parse)
           .ToList();

        var beacons = sensors.Select(x => x.ClosestBeacon)
            .Distinct()
            .ToList();

        var (x, y) = (0L, 0L);

        for (var row = minCoordinate; row <=maxCoordinate; row++)
        {
            var beaconsOnRow = beacons.Count(b => b.Y == row);
            var slices = GetAreaSlicesForRow(sensors, row);

            var minX = minCoordinate;
            
            foreach (var (min, max) in slices)
            {
                if(minX < min)
                {
                    x = min - 1;
                    y = row;
                    break;
                }

                minX = max;
            }

            //it has been found, break;
            if(x != 0)
            {
                break;
            }
        }

        return x * 4000000 + y;
    }

    private List<(long min, long max)> GetAreaSlicesForRow(IList<Sensor> sensors, long row)
    {
        var slices = sensors
            .Select(s => s.SensorAreaSlice(row))
            .Where(s => s.HasValue)
            .Select(s => s!.Value)
            .OrderBy(s => s.min)
            .ToList();

        var i = 1;
        while (i < slices.Count)
        {
            var s1 = slices[i - 1];
            var s2 = slices[i];
            if (Overlapping(s1, s2))
            {
                slices[i - 1] = Combine(s1, s2);
                slices.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        return slices;
    }

    private static (long min, long max) Combine((long min, long max) s1, (long min, long max) s2)
    {
        return (Math.Min(s1.min, s2.min), Math.Max(s1.max, s2.max));
    }

    private static bool Overlapping((long min, long max) lower, (long min, long max) higher)
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

    [Fact]
    public void Part2Dev()
    {
        var result = DetermineTuningFrequency("Aoc.Day15.input.dev.txt", 0, 20);
        Assert.Equal(56000011, result);
    }

    [Fact]
    public void Part2()
    {
        var result = DetermineTuningFrequency("Aoc.Day15.input.txt", 0, 4000000);
        Assert.Equal(12274327017867, result);
    }
}