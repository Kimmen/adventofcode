using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day1;

public class Day1
{
    private readonly ITestOutputHelper _output;

    public Day1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Part1()
    {
        var numbers = Helpers.ReadLinesFromResource("Aoc.Day1.input.1.txt")
            .Select(int.Parse)
            .ToList();

        var increaseCount = 0;
        var lastNumber = numbers.First();
        foreach (var number in numbers)
        {
            if (number > lastNumber)
            {
                increaseCount++;
            }

            lastNumber = number;
        }

        _output.WriteLine($"Increases: {increaseCount}");
    }
    
    [Fact]
    public void Part2()
    {
        var numbers = Helpers.ReadLinesFromResource("Aoc.Day1.input.1.txt")
            .Select(int.Parse)
            .ToList();

        var sums = new List<int>(numbers.Count);
        for (var i = 0; i < numbers.Count - 2; i++)
        {
            sums.Add(numbers[i] + numbers[i + 1] + numbers[i + 2]);
        }

        var increaseCount = 0;
        var lastSum = sums.First();
        foreach (var sum in sums)
        {
            if (sum > lastSum)
            {
                increaseCount++;
            }

            lastSum = sum;
        }

        _output.WriteLine($"Increases: {increaseCount}");
    }
}