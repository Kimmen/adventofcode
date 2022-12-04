using System;
using System.Linq;
using System.Text.RegularExpressions;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day4;

public partial class Puzzle
{
    private readonly ITestOutputHelper _output;
    private static readonly Regex SectionExtractor = SectionExtractorRegex();
   
    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private int DetermineContainedSections(string inputFile)
    {
        var numberOfContainedPairs = Helpers.ReadLinesFromResource(inputFile)
            .Select(ExtractSections)
            .Where(HasContainedSections)
            .Count();

        return numberOfContainedPairs;
    }

    private bool HasContainedSections((Range First, Range Second) sectionPair)
    {
        var (first, second) = sectionPair;

        //_output.WriteLine($"{first.Contains(second)} , {second.Contains(first)}");

        return first.Contains(second) || second.Contains(first);
    }

    private (Range First, Range Second) ExtractSections(string sectionPairDefinition)
    {
        var matches = SectionExtractor.Matches(sectionPairDefinition);
        if (matches.Count != 2) throw new ArgumentException(null, nameof(sectionPairDefinition));

        static Range GenerateRangeFromMatch(Match match)
        {
            var start = int.Parse(match.Groups["Start"].Value);
            var end = int.Parse(match.Groups["End"].Value);

            return new Range(start, end);
        }

        var firstRange = GenerateRangeFromMatch(matches.First());
        var seconRange = GenerateRangeFromMatch(matches.Last());

        //_output.WriteLine($"{firstRange}, {seconRange}");
        return (firstRange, seconRange);
    }

    [Fact]
    public void Part1Dev()
    {
        var containedPairsCount = DetermineContainedSections("Aoc.Day4.input.dev.txt");
        Assert.StrictEqual(2, containedPairsCount);
    }

    [Fact]
    public void Part1()
    {
        var containedPairsCount = DetermineContainedSections("Aoc.Day4.input.txt");
        Assert.StrictEqual(431, containedPairsCount);
        _output.WriteLine($"Contained pairs: {containedPairsCount}");
    }

    [Fact]
    public void Part2Dev()
    {
        var containedPairsCount = DetermineContainedSections("Aoc.Day4.input.dev.txt");
        Assert.StrictEqual(70, containedPairsCount);
    }

    [Fact]
    public void Part2()
    {
        var containedPairsCount = DetermineContainedSections("Aoc.Day4.input.txt");
        Assert.StrictEqual(2738, containedPairsCount);
        _output.WriteLine($"Priority sum: {containedPairsCount}");
    }

    [GeneratedRegex("(?'Start'\\d{1,2})-(?'End'\\d{1,2})", RegexOptions.Compiled)]
    private static partial Regex SectionExtractorRegex();
}