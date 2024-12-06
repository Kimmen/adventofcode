using System.Collections.Concurrent;
using System.Collections.Immutable;
using Spectre.Console;

namespace Aoc.Day5;

public class Part1 : IChallenge
{
    private string _inputExt = "txt";
    private long? _expectedResult = 5964;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 143;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput).ToArray();

        var orderingRules = ParseOrderingRules(lines);
        var pageProduction = ParsePageProduction(lines);

        var middleSum = 0L;

        // Create the layout
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Pages"),
                new Layout("Rules")
                    .SplitRows(
                        new Layout("Preceding"),
                        new Layout("Proceeding")));

        AnsiConsole.Live(layout).Start(ctx =>
        {
            foreach (var pages in pageProduction)
            {
                var processed = new Queue<int>();
                var left = new Queue<int>(pages);

                var productIsValid = false;
                do
                {
                    var current = left.Dequeue();
                    UpdateCurrentPage(layout, pages, current);
                    UpdatePrecedingRules(layout, current, orderingRules.Precedings);
                    UpdateProceedingRules(layout, current, orderingRules.Proceedings);
                    // Thread.Sleep(1);
                    ctx.Refresh();

                    var isCorrectWithPreceding = IsCorrectWithPreceding(current, processed.ToArray(), orderingRules.Precedings);
                    var isCorrectWithProceedings = IsCorrectWithProceedings(current, left.ToArray(), orderingRules.Proceedings);
                    processed.Enqueue(current);

                    productIsValid = isCorrectWithPreceding && isCorrectWithProceedings;

                } while (left.Count != 0 && productIsValid);

                if (productIsValid)
                {
                    var middle = pages[Convert.ToInt32(Math.Floor(pages.Length / 2.0))];
                    middleSum += middle;
                }
            }
        });


        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Successful page middle sum: ");
        PrintResult(middleSum);
    }

    private void UpdatePrecedingRules(Layout layout, int current, IDictionary<int, HashSet<int>> orderingRulesProceedings)
    {
        var visualPages = orderingRulesProceedings.TryGetValue(current, out var precedings)
            ? precedings.Select(page => $"[lightslategrey]{page}[/]").ToArray()
            : ["[grey]None[/]"];

        layout["Rules"]["Preceding"].Update(
            new Panel(
                    Align.Center(
                        new Markup(string.Join(", ", visualPages)),
                        VerticalAlignment.Middle))
                .Expand());
    }

    private void UpdateProceedingRules(Layout layout, int current, IDictionary<int, HashSet<int>> orderingRulesPrecedings)
    {
        var visualPages = orderingRulesPrecedings.TryGetValue(current, out var precedings)
            ? precedings.Select(page => $"[lightslategrey]{page}[/]").ToArray()
            : ["[grey]None[/]"];

        layout["Rules"]["Proceeding"].Update(
            new Panel(
                    Align.Center(
                        new Markup(string.Join(", ", visualPages)),
                        VerticalAlignment.Middle))
                .Expand());
    }

    private void UpdateCurrentPage(Layout layout, int[] pages, int currentPage)
    {
        var visualPages = pages.Select((page) => page == currentPage ? $"[gold3_1]{page}[/]" : page.ToString());
        layout["Pages"].Update(
            new Panel(
                    Align.Center(
                        new Markup(string.Join(", ", visualPages)),
                        VerticalAlignment.Middle))
                .Expand());
    }

    private bool IsCorrectWithPreceding(int current, ICollection<int> processed, IDictionary<int, HashSet<int>> orderingRulesPrecedings)
    {
        if (!orderingRulesPrecedings.TryGetValue(current, out var precedings))
        {
            precedings = [];
        }

        return processed.Count == 0 || precedings.Count == 0 || processed.All(precedings.Contains);
    }

    private bool IsCorrectWithProceedings(int current, ICollection<int> left, IDictionary<int, HashSet<int>> orderingRulesProceedings)
    {
        if (!orderingRulesProceedings.TryGetValue(current, out var proceedings))
        {
            proceedings = [];
        }

        return left.Count == 0 || proceedings.Count == 0 || left.All(proceedings.Contains);
    }

    private ICollection<int[]> ParsePageProduction(IEnumerable<string> lines)
    {
        return lines
            .Where(line => line.Contains(','))
            .Select(line => line
                .Split(',')
                .Select(x => x.Trim())
                .Select(int.Parse)
                .ToArray())
            .ToArray();
    }

    private (IDictionary<int, HashSet<int>> Precedings , IDictionary<int, HashSet<int>> Proceedings) ParseOrderingRules(IEnumerable<string> lines)
    {
        var precedings = new ConcurrentDictionary<int, HashSet<int>>();
        var proceedings = new ConcurrentDictionary<int, HashSet<int>>();

        foreach (var line in lines)
        {
            if(string.IsNullOrEmpty(line))
            {
                break;
            }
            var pages = line.Split('|');
            var precedingPage = int.Parse(pages[0]);
            var proceedingPage = int.Parse(pages[1]);

            var proceedingsForPage = proceedings.GetOrAdd(precedingPage, _ => new HashSet<int>());
            proceedingsForPage.Add(proceedingPage);

            var precedingsForPage = precedings.GetOrAdd(proceedingPage, _ => new HashSet<int>());
            precedingsForPage.Add(precedingPage);
        }

        return (precedings, proceedings);
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
}