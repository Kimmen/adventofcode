using System.Text.RegularExpressions;
using Spectre.Console;

namespace Aoc.Day2;

public partial class Part1 : IChallenge
{
    private string _input = "Aoc.Day2.input.txt";
    private long? _expectedResult = 21139440284L;
    private TimeSpan _delay = TimeSpan.Zero;
    private static readonly Regex Extract = ExtractIdsRegex();

    public void UseDevInput()
    {
        _input = "Aoc.Day2.input.dev.txt";
        _expectedResult = 1227775554L;
    }

    public void SetSpeed(int millisecondsDelay)
    {
        _delay = TimeSpan.FromMilliseconds(millisecondsDelay);
    }

    public void Run()
    {
        var sum = 0L;
        AnsiConsole
            .Progress()
            .AutoRefresh(true)
            .Start(ctx =>
            {
                var content = InputReader.ReadContentFromResource(_input);
                var idRanges = content.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var idRange in idRanges)
                {
                    var (first, last) = ExtractIds(idRange);
                    ArgumentOutOfRangeException.ThrowIfNegative(last - first);
                    var progress = ctx.AddTask($"[green]Spanning {idRange}[/]", maxValue: last-first + 1);
                    
                    for (var x = first; x <= last; x++)
                    {
                        if (IsDuplicate(x))
                        {
                            sum += x;
                        }
                        
                        progress.Increment(1);
                        //ctx.Refresh();
                    }
                }
            });
        
        PrintResult(sum);
    }

    public bool IsDuplicate(long id)
    {
        var value = id.ToString();
        
        if(value.Length % 2 != 0)
        {
            return false;
        }
        
        var i1 = 0;
        var i2 =value.Length / 2;

        while (i2 < value.Length)
        {
            if (value[i1] != value[i2])
            {
                return false;
            }

            i1++;
            i2++;
        }

        return true;
    }

    private static (long first, long last) ExtractIds(string range)
    {
        var match = Extract.Match(range);
        var first = long.Parse(match.Groups["first"].ValueSpan);
        var end = long.Parse(match.Groups["end"].ValueSpan);
        
        return (first, end);
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

    [GeneratedRegex(@"^(?'first'\d+)-(?'end'\d+)$")]
    private static partial Regex ExtractIdsRegex();
}