using System;
using System.Linq;
using System.Text.RegularExpressions;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day2;

public class Day2
{
    private readonly ITestOutputHelper _output;

    public Day2(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Part1()
    {
        var parseCommandRegex = new Regex(@"^(\w+) (\d+)$", RegexOptions.Compiled);

        var commands = Helpers.ReadLinesFromResource("Aoc.Day2.input.1.txt")
            .Select(l => parseCommandRegex.Match(l))
            .Select(m => (Command: m.Groups[1].Value, Argument: int.Parse(m.Groups[2].Value)))
            .ToList();

        var horizontal = 0;
        var depth = 0;

        foreach (var command in commands)
        {
            horizontal += command.Command switch
            {
                "forward" => command.Argument,
                _ => 0
            };

            depth += command.Command switch
            {
                "up" => -command.Argument,
                "down" => command.Argument,
                _ => 0
            };
        }

        var multiplyer = horizontal * depth;

        _output.WriteLine($"Multiplyer: {multiplyer}");
    }

    [Fact]
    public void Part2()
    {
        var parseCommandRegex = new Regex(@"^(\w+) (\d+)$", RegexOptions.Compiled);

        var commands = Helpers.ReadLinesFromResource("Aoc.Day2.input.1.txt")
            .Select(l => parseCommandRegex.Match(l))
            .Select(m => (Command: m.Groups[1].Value, Argument: int.Parse(m.Groups[2].Value)))
            .ToList();

        var horizontal = 0;
        var depth = 0;
        var aim = 0;

        var processUp = (int value) => { aim -= value; };
        var processDown = (int value) => { aim += value; };
        var processForward = (int value) =>
        {
            horizontal += value;
            depth += value * aim;
        };


        foreach (var command in commands)
        {
            Action<int> execute = command.Command switch
            {
                "up" => processUp,
                "down" => processDown,
                "forward" => processForward,
                _ => (int value) => { }
            };

            execute(command.Argument);
        }

        var multiplyer = horizontal * depth;

        _output.WriteLine($"Multiplyer: {multiplyer}");
    }
}