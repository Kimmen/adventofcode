using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day3;

public class Day3
{
    private readonly ITestOutputHelper _output;

    public Day3(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Part1()
    {
        var diagnosticReport = Helpers.ReadLinesFromResource("Aoc.Day3.input.1.txt")
            .ToList();

        var bitRowSize = diagnosticReport.First().Length;

        var bitCounterPerColumn = new (int Ones, int Zeroes) [bitRowSize];

        foreach (var bitRow in diagnosticReport)
        {
            for (int i = 0; i < bitRowSize; i++)
            {
                var counter = bitCounterPerColumn[i];
                var bit = (int)char.GetNumericValue(bitRow[i]);
                if(bit == 1)
                {
                    counter.Ones++;
                }
                else 
                { 
                    counter.Zeroes++; 
                }

                bitCounterPerColumn[i] = counter;
            }
        }

        var gammaRateBits = "";
        var epsilonRateBits = "";

        for (int i = 0; i < bitRowSize; i++)
        {
            var counter = bitCounterPerColumn[i];
            if (counter.Ones > counter.Zeroes)
            {
                gammaRateBits += "1";
                epsilonRateBits += "0";
            }
            else
            {
                gammaRateBits += "0";
                epsilonRateBits += "1";
            }
        }

        var gammaRate = Convert.ToInt32(gammaRateBits, 2);
        var epsilonRate = Convert.ToInt32(epsilonRateBits, 2);

        var powerConsumption = gammaRate * epsilonRate;

        _output.WriteLine($"PowerConsumption: {powerConsumption}");
    }
    
    [Fact]
    public void Part2()
    {
        var diagnosticReport = Helpers.ReadLinesFromResource("Aoc.Day3.input.1.txt")
           .ToList();

        var bitRowSize = diagnosticReport.First().Length;

        var ogReports = diagnosticReport;
        var csReports = diagnosticReport;

        for (int i = 0; i < bitRowSize; i++)
        {
            ogReports = FindNextSet(i, ogReports, count =>
            {
                var (ones, zeroes, bit) = count;
                var bitCompare = count.Ones >= count.Zeroes
                    ? '1'
                    : '0';

                return bit == bitCompare;
            });

            csReports = FindNextSet(i, csReports, count =>
            {
                var (ones, zeroes, bit) = count;
                var bitCompare = count.Zeroes <= count.Ones
                    ? '0'
                    : '1';

                return bit == bitCompare;
            });
        }

        int oxygenGeneratorRating = Convert.ToInt32(ogReports.First(), 2);
        int co2ScrubberRating = Convert.ToInt32(csReports.First(), 2);

        var lifeSupportRating = oxygenGeneratorRating * co2ScrubberRating;

        _output.WriteLine($"LifeSupportRating: {lifeSupportRating}");
    }

    private static List<string> FindNextSet(int bitIndex, List<string> reportRows, Func<(int Ones, int Zeroes, char Bit), bool> match)
    {
        if(reportRows.Count == 1)
        {
            return reportRows;
        }

        var matches = new List<string>();
        var (ones, zeroes) = CountBits(bitIndex, reportRows);

        foreach (var row in reportRows)
        {
            if(match((ones, zeroes, row[bitIndex])))
            {
                matches.Add(row);
            }

        }

        return matches;
    }

    private static (int Ones, int Zeroes) CountBits(int bitIndex, List<string> reportRows)
    {
        var ones = 0;
        var zeroes = 0;

        foreach (var row in reportRows)
        {
            var bit = row[bitIndex];

            if(bit == '1')
            {
                ones++;
            }
            else 
            { 
                zeroes++; 
            }
        }

        return (ones, zeroes);
    }
}