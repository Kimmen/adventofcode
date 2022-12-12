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
            .ChunkBy(string.IsNullOrWhiteSpace)
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
        });

        return inspections
            .OrderDescending()
            .Take(2)
            .Aggregate(1L, (acc, x) => acc * x);
    }

    private long GoodMonkeyReleifer(long x) => x /= 3L;

    private long ModulusArchmeticDevReleifer(long x) 
    {
        // Didn't want to refactor this part, so this is quick and dirty
        // Look at each divisable in input for each monkey
        const int diviserProduct = 23 * 19 * 13 * 17;
        
        return x % diviserProduct;
    }

    private long ModulusArchmeticReleifer(long x)
    {
        // Didn't want to refactor this part, so this is quick and dirty
        // Look at each divisable in input for each monkey
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