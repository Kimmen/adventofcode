using System;
using System.Linq;
using System.Text.RegularExpressions;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day4;

public partial class Puzzle
{
    private readonly ITestOutputHelper _output;
    [GeneratedRegex("(?'Start'\\d{1,2})-(?'End'\\d{1,2})", RegexOptions.Compiled)]
    private static partial Regex SectionExtractorRegex();
    private static readonly Regex SectionExtractor = SectionExtractorRegex();
   
    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    private int CountSections(string inputFile, Func<(Range First, Range Second), bool> sectionsPredicate)
    {
        var numberOfContainedPairs = InputReader.ReadLinesFromResource(inputFile)
            .Select(ExtractSections)
            .Where(sectionsPredicate)
            .Count();

        return numberOfContainedPairs;
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

        return (firstRange, seconRange);
    }

    private bool HasContainedSections((Range First, Range Second) sectionPair)
    {
        var (first, second) = sectionPair;

        return first.Contains(second) || second.Contains(first);
    }

    private bool HasOverlapedSections((Range First, Range Second) sectionPair)
    {
        var (first, second) = sectionPair;

        return first.Overlaps(second);
    }

    [Fact]
    public void Part1Dev()
    {
        var containedPairsCount = CountSections("Aoc.Day4.input.dev.txt", HasContainedSections);
        Assert.StrictEqual(2, containedPairsCount);
    }

    [Fact]
    public void Part1()
    {
        var containedPairsCount = CountSections("Aoc.Day4.input.txt", HasContainedSections);
        Assert.StrictEqual(431, containedPairsCount);
        _output.WriteLine($"Contained pairs: {containedPairsCount}");
    }

    [Fact]
    public void Part2Dev()
    {
        var overlappedPairsCount = CountSections("Aoc.Day4.input.dev.txt", HasOverlapedSections);
        Assert.StrictEqual(4, overlappedPairsCount);
    }

    [Fact]
    public void Part2()
    {
        var overlappedPairsCount = CountSections("Aoc.Day4.input.txt", HasOverlapedSections);
        Assert.StrictEqual(823, overlappedPairsCount);
        _output.WriteLine($"Contained pairs: {overlappedPairsCount}");
    }
}