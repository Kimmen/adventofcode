using System.Collections.Concurrent;
using Spectre.Console;

namespace Aoc.Day1;

public class Day1Part2 : IChallenge
{
    private string _input = "Aoc.Day1.input.txt";
    private long? _expectedResult;

    public void UseDevInput()
    {
        _input = "Aoc.Day1.input.dev.txt";
        _expectedResult = 31L;
    }

    public void Run()
    {
        var locationTable = new Table
        {
            Border = TableBorder.MinimalHeavyHead
        };

        List<int> locationIds = new(1000);
        ConcurrentDictionary<int, int> locationOccurrences = new();
        List<int> similarityScores = new(1000);

        AnsiConsole.Status()
            .Start("Reading locations...", _ =>
            {
                foreach (var line in InputReader.StreamLines(_input))
                {
                    var parsed = line.Split("   ");
                    locationIds.Add(int.Parse(parsed[0]));
                    locationOccurrences.AddOrUpdate(int.Parse(parsed[1]), 1, (_, occurrences) => occurrences + 1);
                    Thread.Sleep(10);
                }
            });

        Thread.Sleep(500);

        var processTable = new Table
        {
            Border = TableBorder.MinimalHeavyHead
        };
        processTable
            .AddColumn("Location")
            .AddColumn("Occurrences")
            .AddColumn("Similarity score (0)")
            .AddEmptyRow();

        AnsiConsole.Live(processTable)
            .Start(ctx =>
            {
                foreach (var locationId in locationIds)
                {
                    var occurrences = locationOccurrences.GetValueOrDefault(locationId, 0);
                    var similarityScore = occurrences * locationId;
                    similarityScores.Add(similarityScore);

                    processTable.Rows.Update(0, 0, Markup.FromInterpolated($"[green]{locationId}[/]"));
                    processTable.Rows.Update(0, 1, Markup.FromInterpolated($"[yellow]{occurrences}[/]"));
                    processTable.Rows.Update(0, 2, Markup.FromInterpolated($"[green]{similarityScore}[/]"));
                    processTable.Columns[2].Header($"Similarity score ({similarityScores.Sum()})");
                    ctx.Refresh();
                    Thread.Sleep(10);
                }

                Thread.Sleep(500);
            });

        var result = similarityScores.Sum();
        AnsiConsole.MarkupLine($"Total similarity score:");
        PrintResult(result);
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