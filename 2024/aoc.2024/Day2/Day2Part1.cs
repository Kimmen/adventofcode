using Spectre.Console;

namespace Aoc.Day2;

public class Day2Part1 : IChallenge
{
    private string _inputExt = "txt";
    private long? _expectedResult = 670L;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 2L;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var reports = new List<(int[] report, bool isSafe)>();

        var reportsTable = new Table
            {
                Border = TableBorder.MinimalHeavyHead
            }
            .Title("Reports")
            .AddColumn("Reports")
            .AddRow(GenerateBar(0, 0));

        AnsiConsole.Live(reportsTable)
            .Start(ctx =>
            {
                var safes = 0;
                var unsafes = 0;
                foreach (var line in InputReader.StreamLines(PuzzleInput))
                {
                    var report = line.Split(" ").Select(int.Parse).ToArray();
                    var isSafe = IsSafeReport(report);
                    reports.Add((report, isSafe));

                    safes += isSafe ? 1 : 0;
                    unsafes += isSafe ? 0 : 1;

                    reportsTable.UpdateCell(0, 0, GenerateBar(safes, unsafes));

                    Thread.Sleep(10);
                    ctx.Refresh();
                }
            });

        var safeReports = reports.Sum(x => x.isSafe ? 1 : 0);
        AnsiConsole.MarkupLine("Total safe reports: ");
        PrintResult(safeReports);
    }

    private static bool IsSafeReport(int[] report)
    {
        return DetectError(report) == -1;

        int DetectError(int[] levels)
        {
            var changeRatio = Math.Sign(levels[1] - levels[0]);

            for (var i = 0; i < levels.Length - 1; i++)
            {
                var first = levels[i];
                var second = levels[i + 1];
                var difference = second - first;
                var distance = Math.Abs(difference);

                var safe = difference switch
                {
                    0 => false,
                    _ when Math.Sign(difference) != changeRatio => false,
                    _ when distance is < 1 or > 3 => false,
                    _ => true
                };

                if(!safe)
                {
                    return i;
                }
            }

            return -1;
        }
    }

    private BarChart GenerateBar(int safeCount, int unsafeCount)
    {
        return new BarChart()
            .Width(60)
            .WithMaxValue(Math.Max(10, safeCount + unsafeCount))
            .AddItem("Safe", safeCount, Color.Green)
            .AddItem("Unsafe", unsafeCount, Color.Red);
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