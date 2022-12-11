using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

using static Aoc.LoopUtilities;

namespace Aoc.Day11;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private long DetermineMonkeyBusiness(string input, int rounds, Func<long, long> reliefer)
    {
        var monkeys = InputReader.ReadLinesFromResource(input)
            .ChunkBy(x => string.IsNullOrWhiteSpace(x))
            .Select(Monkey.Parse)
            .ToArray();

        var inspections = Enumerable.Repeat(0L, monkeys.Length).ToList();

        Repeat(rounds, (round) =>
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.TryDequeue(out var worryLevel))
                {
                    var newWorryLevel = reliefer(monkey.Operation(worryLevel));
                    var throwToMonkeyIndex = monkey.Test(newWorryLevel);

                    monkeys[throwToMonkeyIndex].Items.Enqueue(newWorryLevel);
                    inspections[monkey.Index]++;
                }
            }

            var printInspections = (round + 1) switch
            {
                1 => true,
                20 => true,
                1000 => true,
                2000 => true,
                3000 => true,
                4000 => true,
                5000 => true,
                6000 => true,
                7000 => true,
                8000 => true,
                9000 => true,
                10000 => true,
                _ => false
            };

            if (printInspections)
            {
                _output.WriteLine($"== After round {round + 1} ==");
                PrintInspections(inspections);
            }
        });
        
        PrintInspections(inspections);

        return inspections
            .OrderDescending()
            .Take(2)
            .Aggregate(1L, (acc, x) => acc * x);
    }

    private void PrintInspections(List<long> inspections)
    {
        for (int i = 0; i < inspections.Count; i++)
        {
            _output.WriteLine($"Monkey {i} inspected items {inspections[i]} times.");
        }

        _output.WriteLine("---");
    }

    private long GoodMonkeyReleifer(long x) => x /= 3L;

    private long ModulusArchmeticDevReleifer(long x) 
    {
        //Look at each divisable in input for each monkey
        const int diviserProduct = 23 * 19 * 13 * 17;
        
        return x % diviserProduct;
    }

    private long ModulusArchmeticReleifer(long x)
    {
        //Look at each divisable in input for each monkey
        const int diviserProduct = 5 * 17 * 7 * 13 * 19 * 3 * 11 * 2;

        return x % diviserProduct;
    }

    [Fact]
    public void Part1Dev()
    {
        var monkeyBusiness = DetermineMonkeyBusiness("Aoc.Day11.input.dev.txt", 20, GoodMonkeyReleifer);
        Assert.Equal(10605L, monkeyBusiness);
    }

    [Fact]
    public void Part1()
    {
        var monkeyBusiness = DetermineMonkeyBusiness("Aoc.Day11.input.txt", 20, GoodMonkeyReleifer);
        Assert.Equal(54054L, monkeyBusiness);
    }

    [Fact]
    public void Part2Dev()
    {
        var monkeyBusiness = DetermineMonkeyBusiness("Aoc.Day11.input.dev.txt", 10000, ModulusArchmeticDevReleifer);
        Assert.Equal(2713310158L, monkeyBusiness);
    }

    [Fact]
    public void Part2()
    {
        var monkeyBusiness = DetermineMonkeyBusiness("Aoc.Day11.input.txt", 10000, ModulusArchmeticReleifer);
        Assert.Equal(14314925001L, monkeyBusiness);
    }
}