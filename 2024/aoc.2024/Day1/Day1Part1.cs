using Spectre.Console;

namespace Aoc.Day1;

public class Day1Part1 : IChallenge
{
    private string _input = "Aoc.Day1.input.txt";
    private long? _expectedResult = 1258579L;

    public void UseDevInput()
    {
        _input = "Aoc.Day1.input.dev.txt";
        _expectedResult = 11L;
    }

    public void Run()
    {
        var locationTable = new Table
        {
            Border = TableBorder.MinimalHeavyHead
        };

        locationTable.AddColumn("Left").AddColumn("Right").AddColumn("Distance");

        var leftLocationIds = new List<int>(1000);
        var rightLocationIds = new List<int>(1000);
        var distances = new List<int>(1000);

        AnsiConsole.Live(locationTable)
            .Start(ctx =>
            {
                var counter = 0;
                foreach (var line in InputReader.StreamLines(_input))
                {
                    var locationIds = line.Split("   ");
                    leftLocationIds.Add(int.Parse(locationIds[0]));
                    rightLocationIds.Add(int.Parse(locationIds[1]));

                    locationTable.AddRow(locationIds[0], locationIds[1]);

                    if (++counter % 50 == 0)
                    {
                        ctx.Refresh();
                    }
                }

                Thread.Sleep(500);

                leftLocationIds.Sort();
                rightLocationIds.Sort();

                for (var i = 0; i < locationTable.Rows.Count; i++)
                {
                    var leftId = leftLocationIds[i];
                    var rightId = rightLocationIds[i];
                    var distance = Math.Abs(leftId - rightId);

                    distances.Add(distance);
                    locationTable.Rows.Update(i, 0, Markup.FromInterpolated($"[green]{leftId}[/]"));
                    locationTable.Rows.Update(i, 1, Markup.FromInterpolated($"[green]{rightId}[/]"));
                    locationTable.Rows.Update(i, 2, Markup.FromInterpolated($"[yellow]{distance}[/]"));
                    locationTable.Columns[2].Header = Markup.FromInterpolated($"[yellow]Total: {distances.Sum()}[/]");
                    if (i % 50 == 0)
                    {
                        ctx.Refresh();
                    }

                }
                Thread.Sleep(500);
            });

        var totalDistance = distances.Sum();

        AnsiConsole.MarkupLine("Total distance: ");
        PrintResult(totalDistance);
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