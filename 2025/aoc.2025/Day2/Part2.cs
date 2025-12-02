using System.Text.RegularExpressions;
using Spectre.Console;

namespace Aoc.Day2;

public partial class Part2 : IChallenge
{
    private string _input = "Aoc.Day2.input.txt";
    private long? _expectedResult = 38731915928L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;
    private static readonly Regex Extract = ExtractIdsRegex();

    public void UseDevInput()
    {
        _input = "Aoc.Day2.input.dev.txt";
        _expectedResult = 4174379265L;
    }

    public void RefreshRate(long rate)
    {
        _refreshRate = rate;
    }

    public void Run()
    {
        var sum = 0L;
        
        AnsiConsole
            .Progress()
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
                        if (IncludesRepeatingPattern(x))
                        {
                            sum += x;
                        }
                        
                        progress.Increment(1);
                        if (DoRefresh())
                        {
                            ctx.Refresh();    
                        }
                        
                    }
                }
            });
        
        PrintResult(sum);
    }

    private static bool IncludesRepeatingPattern(long id)
    {
        var value = id.ToString();
        var chunkSize = 1;

        while (chunkSize <= value.Length)
        {
            var chunk = value[..chunkSize];
            var additionalChunkSizeSum = 0;

            for (var i = value.IndexOf(chunk, chunkSize, StringComparison.Ordinal); i > -1; i = value.IndexOf(chunk, chunkSize + i, StringComparison.Ordinal))
            {
                additionalChunkSizeSum += chunkSize;
            }
            
            //Only found one then there are no repeats
            if (additionalChunkSizeSum == 0)
            {
                return false;
            }

            //Does all chunk sizes add upp to the total id size?
            if (additionalChunkSizeSum + chunkSize == value.Length)
            {
                return true;
            }
            chunkSize++;
        }

        return false;
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

    private bool DoRefresh() => _refreshCount++ % _refreshRate == 0;

    [GeneratedRegex(@"^(?'first'\d+)-(?'end'\d+)$")]
    private static partial Regex ExtractIdsRegex();
}