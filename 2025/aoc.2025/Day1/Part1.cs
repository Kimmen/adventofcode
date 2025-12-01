using System.Text.RegularExpressions;
using Spectre.Console;

namespace Aoc.Day1;

public partial class Part1 : IChallenge
{
    private string _input = "Aoc.Day1.input.txt";
    private long? _expectedResult = 1154L;
    private TimeSpan _delay = TimeSpan.Zero;

    public void UseDevInput()
    {
        _input = "Aoc.Day1.input.dev.txt";
        _expectedResult = 3L;
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

        locationTable.AddColumn("Position").AddColumn("Dail Rotation").AddColumn("Target");
        var regex = DailRotationMatch();
        var position = 50L;
        var maxPosition = 99L;
        var modulusCap = maxPosition + 1;

        var matchCount = 0;

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
                    steps %= modulusCap; // optimize steps within range

                    var newPosition = dir switch
                    {
                        'L' => MoveLeft(position, steps, modulusCap),
                        'R' => MoveRight(position, steps, modulusCap),
                        _ => throw new InvalidOperationException($"Unknown direction: {dir}")
                    };

                    if (newPosition < 0)
                    {
                        throw new InvalidOperationException($"Position went below minimum: {position} -> {newPosition}");
                    }

                    locationTable.AddRow(position.ToString(), line, newPosition.ToString());
                    position = newPosition;
                    if(IsMatch(position))
                    {
                        matchCount++;
                    }


                    Thread.Sleep(_delay);
                }
            });

        AnsiConsole.MarkupLine("Total matches: ");
        PrintResult(matchCount);
        return;

        bool IsMatch(long p) => p == 0;
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