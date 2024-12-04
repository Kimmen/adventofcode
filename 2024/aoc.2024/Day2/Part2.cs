using Spectre.Console;

namespace Aoc.Day2;

public class Part2 : IChallenge
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
            var isSafe = index == -1;

            if (!isSafe)
            {
                PrintReport(report, -1, index, reason, false);

                for (var i = 0; i < report.Length; i++)
                {
                    var testReport = i == -1 ? report : report.Where((_, j) => j != i).ToArray();
                    var (ix, r) = DetectError(testReport);
                    isSafe = ix == -1;

                    PrintReport(report, i, ix, r, true);

                    if (isSafe)
                    {
                        break;
                    }
                }
            }

            if (isSafe)
            {
                safeReports++;
            }
        }

        AnsiConsole.MarkupLine("Total safe reports: ");
        PrintResult(safeReports);
    }

    private void PrintReport(int[] report, int removedIndex, int errorIndex, string reason, bool indent)
    {
        var isSafeVisual = string.IsNullOrEmpty(reason) ? $"[green]Ok[/]" : $"[red]No[/]";
        var reportVisual = string.Join(", ", report
            .Select((x, i) =>
            {
                if(i == removedIndex)
                {
                    return $"[lightgoldenrod2_2 bold]{x}[/]";
                }

                if(i == errorIndex)
                {
                    return $"[red underline]{x}[/]";
                }

                return x.ToString();
            }));
        var indentVisual = indent ? "\t" : string.Empty;
        AnsiConsole.MarkupLine($"{indentVisual}{isSafeVisual} [deepskyblue4_1]<{reportVisual}>[/] {reason}");
    }

    private static (int Index, string Reason) DetectError(int[] levels)
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

            if (!safe)
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