using Spectre.Console;

namespace Aoc.Day2;

public class Day2Part2 : IChallenge
{
    private string _inputExt = "txt";
    private long? _expectedResult = null;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 4L;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var safeReports = 0;

        foreach (var line in InputReader.StreamLines(PuzzleInput))
        {
            var report = line.Split(" ").Select(int.Parse).ToArray();

            var (index, reason) = DetectError(report);
            PrintReport(report, index, reason, false);
            var isSafe = index == -1;
            if (isSafe)
            {
                safeReports++;
                Thread.Sleep(10);
                continue;
            }

            var adjustment = report.Where((_, i) => i != index).ToArray();
            var (index1, reason1) = DetectError(adjustment);
            PrintReport(adjustment, index1, reason1, true);
            isSafe = index1 == -1;
            if (isSafe)
            {
                safeReports++;
                Thread.Sleep(10);
                continue;
            }

            var adjustmentPlus = report.Where((_, i) => i != index + 1).ToArray();
            var (index2, reason2) = DetectError(adjustmentPlus);
            PrintReport(adjustmentPlus, index2, reason2, true);
            isSafe = index2 == -1;
            if (isSafe)
            {
                safeReports++;
                Thread.Sleep(10);
            }
        }

        AnsiConsole.MarkupLine("Total safe reports: ");
        PrintResult(safeReports);
    }

    private void PrintReport(int[] report, int errorIndex, string reason, bool indent)
    {
        var isSafeVisual = errorIndex == -1 ? $"[green]Ok[/]" : $"[red]No[/]";
        var reportVisual = string.Join(", ", report.Select((x, i) => i == errorIndex ? $"[lightgoldenrod2_2 bold]{x}[/]" : x.ToString()));
        var indentVisual = indent ? "\t" : string.Empty;
        AnsiConsole.MarkupLine($"{indentVisual}{isSafeVisual} [deepskyblue4_1]<{reportVisual}>[/] {reason}");
    }

    private (int Index, string Reason) DetectError(int[] levels)
    {
        var changeRatio = Math.Sign(levels[1] - levels[0]);

        for (var i = 0; i < levels.Length - 1; i++)
        {
            var first = levels[i];
            var second = levels[i + 1];
            var difference = second - first;
            var distance = Math.Abs(difference);

            var (safe, reason) = difference switch
            {
                0 => (false, "No change"),
                _ when Math.Sign(difference) != changeRatio => (false, "Change ratio"),
                _ when distance is < 1 or > 3 => (false, "Distance to large"),
                _ => (true, string.Empty)
            };

            if(!safe)
            {
                return (i, reason);
            }
        }

        return (-1, string.Empty);
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