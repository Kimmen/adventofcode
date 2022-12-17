using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;

using Xunit;
using Xunit.Sdk;

namespace Aoc.Day16;

public class Puzzle
{

    private long CalculateBestTotalPressureRelease(string input, string valveStart, int minutesLeft)
    {
        var valves = InputReader
            .ReadLinesFromResource(input)
            .Select(Valve.Parse)
            .ToDictionary(x => x.Name);


        var distanceToValve = new Dictionary<(string from, string to), int>(10_000);
        foreach (var from in valves.Values)
        {
            foreach(var to in valves.Values)
            {
                var path = BFS.GetShortestPath(from, to, valves);

                distanceToValve[(from.Name, to.Name)] = path.Count - 1;
            }
        }

        return 0;
       
        //var pressureReleases = new List<long>();

        //var valve = valves[valveStart]; 
        //while(minutesLeft > 0)
        //{
        //    var result = OpenNextValve(valve, valves, minutesLeft);
        //    if(!result.HasValue)
        //    {
        //        break;
        //    }

        //    var (destination, pressureRelease, minutes) = result.Value;

        //    pressureReleases.Add(pressureRelease);
        //    valve = destination;
        //    minutesLeft = minutes;
        //}

        //return pressureReleases.Sum();
    }

    private long FindPath(Valve start, IDictionary<string, Valve> valves, IDictionary<(string from, string to), int> distances, int minutesLeft)
    {
        var keyValves = valves.Values.Where(v => v.Rate > 0).ToList();
        var queue = new Queue<(Valve valve, List<Valve> valvesLeft, int minutes, bool completed, List<Valve> path, long pressureRelease)>();
        queue.Enqueue((start, keyValves, minutesLeft, false, new List<Valve>(), 0));

        while(queue.Any())
        {
            var c = queue.Dequeue();
            if(c.minutes <= 0)
            {
                c.completed = true;
            }
        }
    }

    private (Valve opened, long pressureRelease, int minutesLeft)? OpenNextValve(Valve valve, IDictionary<string, Valve> valves, int minuteMark)
    {
        var visited = new HashSet<string>();
        var openValves = valves.Values
            .Where(x => x.Rate > 0)
            .Where(x => !x.Open)
            .ToList();

        if(!openValves.Any())
        {
            return null;
        }

        var bestFit = openValves
            .Select(v => BFS.GetShortestPath(v, valve, valves))
            .Select(path =>
            {
                var (pressureRelease, minutesLeft) = CalculatePressureRelease(path, minuteMark);
                return (valve: path.Last(), pressureRelease, minuteMark);
            })
            .OrderBy(x => x.pressureRelease)
            .First();

        bestFit.valve.Open = true;

        return bestFit;

        //var visited = new HashSet<string>();
        //var queue = new Queue<(Valve valve, int minute)>();
        //queue.Enqueue((valve, minuteMark));
        //visited.Add(valve.Name);

        //var bestPressureRelease = (Valve: valve, PressureRelease: 0L, Minute: minuteMark);
        //while (queue.Any())
        //{
        //    var (current, minute) = queue.Dequeue();

        //    if (!current.Open)
        //    {
        //        var pressureRelease = current.Rate * (minute - 1);
        //        if (pressureRelease > bestPressureRelease.PressureRelease)
        //        {
        //            bestPressureRelease = (current, pressureRelease, minute - 1);
        //        }
        //    }

        //    var adjacentValves = current.ConnectedValves
        //        .Where(v => !visited.Contains(v))
        //        .Select(v => valves[v])
        //        .ToList();

        //    foreach (var av in adjacentValves)
        //    {
        //        visited.Add(av.Name);
        //        queue.Enqueue((av, minute - 1));
        //    }

        //}

        //var (valveToOpen, bestPressure, min) = bestPressureRelease;

        //valveToOpen.Open = true;

        //return (min, bestPressure);
    }

    private (long pressureRelease, int minutesLeft) CalculatePressureRelease(IList<Valve> path, int minutesLeft)
    {
        var minutesToMoveToAndOpenValve = path.Count;
        minutesLeft -= minutesToMoveToAndOpenValve;
        var rate = path.Last().Rate;

        return (rate * minutesLeft, minutesLeft);
    }

    [Fact]
    public void Part1Dev()
    {
        var result = CalculateBestTotalPressureRelease("Aoc.Day16.input.dev.txt", "AA", 30);
        //Assert.Equal(1651, result);
    }

    [Fact]
    public void Part1()
    {
        var result = CalculateBestTotalPressureRelease("Aoc.Day16.input.txt", "AA", 30);
        //Assert.Equal(1651, result);
    }

    //[Fact]
    //public void Part2Dev()
    //{
    //    var result = DetermineTuningFrequency("Aoc.Day15.input.dev.txt", 0, 20);
    //    Assert.Equal(56000011, result);
    //}

    //[Fact]
    //public void Part2()
    //{
    //    var result = DetermineTuningFrequency("Aoc.Day15.input.txt", 0, 4000000);
    //    Assert.Equal(12274327017867, result);
    //}
}