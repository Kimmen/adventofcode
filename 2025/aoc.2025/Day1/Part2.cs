using System.Text.RegularExpressions;
using Spectre.Console;

namespace Aoc.Day1;

public partial class Part2 : IChallenge
{
    private string _input = "Aoc.Day1.input.txt";
    private long? _expectedResult = 6819L;
    private TimeSpan _delay = TimeSpan.Zero;

    public void UseDevInput()
    {
        _input = "Aoc.Day1.input.dev.txt";
        _expectedResult = 6L;
    }

    public void SetSpeed(int millisecondsDelay)
    {
        _delay = TimeSpan.FromMilliseconds(millisecondsDelay);
    }

    public void Run()
    {
        var locationTable = new Table
        {
            Border = TableBorder.MinimalHeavyHead,
        };

        locationTable
            .AddColumn("Position")
            .AddColumn("Dail Rotation")
            .AddColumn("Target")
            .AddColumn("#Rotations")
            .AddColumn("#Passes")
            .AddColumn("#Visits to 0");
        var regex = DailRotationMatch();
        var position = 50L;
        var maxPosition = 99L;
        var modulusCap = maxPosition + 1;

        var matchCount = 0L;

        AnsiConsole.Live(locationTable)
            .Start(ctx =>
            {
                foreach (var line in InputReader.StreamLines(_input))
                {
                    if(locationTable.Rows.Count % 30 == 0)
                    {
                        locationTable.Rows.Clear();
                    }

                    var (dir, steps) = ParseLine(regex, line);
                    var normalizedSteps = steps % modulusCap; // optimize steps within range
                    var fullRotations = steps / modulusCap;

                    var newPosition = dir switch
                    {
                        'L' => MoveLeft(position, normalizedSteps, modulusCap),
                        'R' => MoveRight(position, normalizedSteps, modulusCap),
                        _ => throw new InvalidOperationException($"Unknown direction: {dir}")
                    };

                    if (newPosition < 0)
                    {
                        throw new InvalidOperationException($"Position went below minimum: {position} -> {newPosition}");
                    }

                    var passesZero = dir switch
                    {
                        _ when newPosition == 0 => 0,
                        _ when position == 0 => 0,
                        'L' when (position - normalizedSteps) < 0 =>  1,
                        'R' when (position + normalizedSteps) > maxPosition =>  1,
                        _ => 0
                    };

                    var visitedZeroes = fullRotations + passesZero + (newPosition == 0 ? 1 : 0);

                    locationTable.AddRow(
                        position.ToString(),
                        line,
                        newPosition.ToString(),
                        fullRotations.ToString(),
                        passesZero.ToString(),
                        visitedZeroes.ToString());

                    position = newPosition;
                    matchCount += visitedZeroes;

                    ctx.Refresh();
                    Thread.Sleep(_delay);
                }
            });

        AnsiConsole.MarkupLine("Total matches: ");
        PrintResult(matchCount);
    }

    private long MoveRight(long position, long steps, long modulus)
    {
        return (position + steps) % modulus;
    }

    private long MoveLeft(long position, long steps, long modulus)
    {
        return (position - steps + modulus) % modulus;
    }

    private (char dir, long steps) ParseLine(Regex regex, string line)
    {
        if (!regex.IsMatch(line))
        {
            throw new InvalidOperationException($"Could not parse line: {line}");
        }

        var match = regex.Match(line);
        var dir = match.Groups["dir"].Value[0];
        var steps = long.Parse(match.Groups["steps"].Value);
        return (dir, steps);
    }

    private void PrintResult(long result)
    {
        var color = _expectedResult.HasValue ? "green" : "yellow";
        if (_expectedResult.HasValue && result != _expectedResult)
        {
            color = "red";
        }

        AnsiConsole.MarkupLine($"[{color} bold]{result}[/]");
    }

    [GeneratedRegex(@"^(?'dir'L|R)(?'steps'\d+)$")]
    private static partial Regex DailRotationMatch();
}