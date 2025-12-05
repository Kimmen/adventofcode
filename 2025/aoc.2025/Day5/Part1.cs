using Spectre.Console;

namespace Aoc.Day5;

public partial class Part1 : IChallenge
{
    private string _input = "txt";
    private long? _expectedResult = 1457L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;

    public void UseDevInput()
    {
        _input = "dev.txt";
        _expectedResult = 3L;
    }

    public void RefreshRate(long rate)
    {
        _refreshRate = rate;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_input}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput);
        var (freshIngredients, availableIngredients) = ParseIngredients(lines);

        var sum = availableIngredients.LongCount(id => IsFresh(id, freshIngredients));

        PrintResult(sum);
    }

    private static bool IsFresh(long id, List<(long start, long end)> freshIngredients)
    {
        foreach (var freshRange in freshIngredients)
        {
            if(id >= freshRange.start && id <= freshRange.end)
            {
                return true;
            }
        }

        return false;

        // var index = freshIngredients.Count / 2;
        // var visited = new HashSet<(long, long)>(freshIngredients.Count);
        // var pivot = freshIngredients[index];
        // while(!visited.Contains(pivot))
        // {
        //     visited.Add(pivot);
        //     if (id >= pivot.end)
        //     {
        //         index = Math.Min(freshIngredients.Count - 1, index + Math.Max(1, index / 2));
        //     }
        //     else if (id <= pivot.start)
        //     {
        //         index = Math.Max(0, index - Math.Max(1, index / 2));
        //     }
        //     else
        //     {
        //         return true;
        //     }
        //     pivot = freshIngredients[index];
        // }
        //
        // return false;
    }

    private (List<(long start, long end)> freshIngredients, List<long> availableIngredients) ParseIngredients(IEnumerable<string> lines)
    {
        using var enumerator = lines.GetEnumerator();
        var freshIngredients = new List<(long start, long end)>(200);
        var availableIngredients = new List<long>(1000);

        while(enumerator.MoveNext() && !string.IsNullOrWhiteSpace(enumerator.Current))
        {
            var parts = enumerator.Current.Split('-');
            freshIngredients.Add((long.Parse(parts[0]), long.Parse(parts[1])));
        }

        freshIngredients.Sort((a, b) => a.end.CompareTo(b.end));

        while(enumerator.MoveNext())
        {
            availableIngredients.Add(long.Parse(enumerator.Current));
        }

        return (freshIngredients, availableIngredients);
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