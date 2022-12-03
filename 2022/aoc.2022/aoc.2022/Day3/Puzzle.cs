using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day3;

using Items = IEnumerable<char>;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

   
    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private int CalculateSharedItemPrioritySum(string inputName, Func<IEnumerable<Items>, IEnumerable<Items[]>> rucksackChunker)
    {
        var rucksacks = Helpers.ReadLinesFromResource(inputName);
        var prioritySum = rucksackChunker(rucksacks)
            .Select(GetSharedItemType)
            .Select(DeterminePriority)
            .Sum();

        return prioritySum;
    }

    private int DeterminePriority(char i)
    {
        var score = 0;
        if(char.IsLower(i))
        {
            score = i - 'a' + 1;
        }
        else if(char.IsUpper(i))
        {
            score = i - 'A' + 27;
        }

        return score;
    }

    private char GetSharedItemType(IEnumerable<Items> rucksacks)
    {
        var sharedItem = rucksacks
            .Skip(1)
            .Aggregate(rucksacks.First().AsEnumerable(), (sharedItems, items) => sharedItems.Intersect(items))
            .First();

        return sharedItem;
    }

    private IEnumerable<Items[]> ChunkByCompartment(IEnumerable<Items> rucksacks)
    {
        return rucksacks.Select(x => x.Chunk(x.Count() / 2).ToArray());
    }

    private IEnumerable<Items[]> ChunkByGroupOfThree(IEnumerable<Items> rucksacks)
    {
        return rucksacks.Chunk(3);
    }

    [Fact]
    public void Part1Dev()
    {
        var prioritySum = CalculateSharedItemPrioritySum("Aoc.Day3.input.dev.txt", ChunkByCompartment);
        Assert.StrictEqual(157, prioritySum);
    }

    [Fact]
    public void Part1()
    {
        var prioritySum = CalculateSharedItemPrioritySum("Aoc.Day3.input.txt", ChunkByCompartment);
        Assert.StrictEqual(8109, prioritySum);
        _output.WriteLine($"Priority sum: {prioritySum}");
    }

    [Fact]
    public void Part2Dev()
    {
        var prioritySum = CalculateSharedItemPrioritySum("Aoc.Day3.input.dev.txt", ChunkByGroupOfThree);
        Assert.StrictEqual(70, prioritySum);
    }

    [Fact]
    public void Part2()
    {
        var prioritySum = CalculateSharedItemPrioritySum("Aoc.Day3.input.txt", ChunkByGroupOfThree);
        Assert.StrictEqual(2738, prioritySum);
        _output.WriteLine($"Priority sum: {prioritySum}");
    }
}