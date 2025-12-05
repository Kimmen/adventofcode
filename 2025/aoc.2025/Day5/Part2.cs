using Spectre.Console;

namespace Aoc.Day5;

public partial class Part2 : IChallenge
{
    private string _input = "txt";
    private long? _expectedResult = 1457L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;

    public void UseDevInput()
    {
        _input = "dev.txt";
        _expectedResult = 14L;
    }

    public void RefreshRate(long rate)
    {
        _refreshRate = rate;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_input}";

    public void Run()
    {
        var sum = 0L;
        var lines = InputReader.StreamLines(PuzzleInput);

        var freshIngredients = ParseFreshIngredients(lines);
        freshIngredients = CombineRanges(freshIngredients);

        foreach (var fresh in freshIngredients)
        {
            sum += fresh.end - fresh.start + 1;
        }

        PrintResult(sum);
    }

    private List<(long start, long end)> CombineRanges(List<(long start, long end)> freshIngredients)
    {
        var didCombine = true;
        while (didCombine)
        {
            didCombine = false;
            for (var i = 0; i < freshIngredients.Count; i++)
            {
                var current = freshIngredients[i];
                for (var j = i + 1; j < freshIngredients.Count; j++)
                {
                    var compare = freshIngredients[j];
                    if (current.start <= compare.end && compare.start <= current.end)
                    {
                        var combined = (Math.Min(current.start, compare.start), Math.Max(current.end, compare.end));
                        freshIngredients.RemoveAt(j);
                        freshIngredients.RemoveAt(i);
                        freshIngredients.Add(combined);
                        didCombine = true;
                        break;
                    }
                }

                if (didCombine)
                {
                    break;
                }
            }
        }

        return freshIngredients;
    }

    private static List<(long start, long end)> ParseFreshIngredients(IEnumerable<string> lines)
    {
        using var enumerator = lines.GetEnumerator();
        var freshIngredients = new List<(long start, long end)>(200);

        while(enumerator.MoveNext() && !string.IsNullOrWhiteSpace(enumerator.Current))
        {
            var parts = enumerator.Current.Split('-');
            freshIngredients.Add((long.Parse(parts[0]), long.Parse(parts[1])));
        }

        return freshIngredients;
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
}