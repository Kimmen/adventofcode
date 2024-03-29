using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day1;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private static long CalculateTotalCalories(string inputName, int includeTopCount)
    {
        var caloriesPerInventory = InputReader.ReadLinesFromResource(inputName)
            .ChunkBy(string.IsNullOrWhiteSpace);

        var totalCalories = caloriesPerInventory
            .Select(x => x.Select(long.Parse).Sum())
            .OrderDescending()
            .Take(includeTopCount)
            .Sum();

        return totalCalories;
    }
    
    [Fact]
    public void Part1Dev()
    {
        var maxTotalCalorieInventory = CalculateTotalCalories("Aoc.Day1.input.dev.txt", 1);
        Assert.StrictEqual(24000L, maxTotalCalorieInventory);
    }
    
    [Fact]
    public void Part1()
    {
        var maxTotalCalorieInventory = CalculateTotalCalories("Aoc.Day1.input.txt", 1);
        _output.WriteLine($"Largest total calorie inventory: {maxTotalCalorieInventory}");
        Assert.StrictEqual(66616L, maxTotalCalorieInventory);
    }
    
    [Fact]
    public void Part2Dev()
    {
        var maxTotalCalorieInventory = CalculateTotalCalories("Aoc.Day1.input.dev.txt", 3);
        Assert.StrictEqual(45000L, maxTotalCalorieInventory);
    }
    
    [Fact]
    public void Part2()
    {
        var maxTotalCalorieInventory = CalculateTotalCalories("Aoc.Day1.input.txt", 3);
        _output.WriteLine($"Largest total calorie inventory: {maxTotalCalorieInventory}");
        Assert.StrictEqual(199172L, maxTotalCalorieInventory);
    }
}
