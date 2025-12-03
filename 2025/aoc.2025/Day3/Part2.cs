using System.Text;
using Spectre.Console;

namespace Aoc.Day3;

public class Part2 : IChallenge
{
    private string _input = "Aoc.Day3.input.txt";
    private long? _expectedResult = 170371185255900L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;

    public void UseDevInput()
    {
        _input = "Aoc.Day3.input.dev.txt";
        _expectedResult = 3121910778619L;
    }

    public void RefreshRate(long rate)
    {
        _refreshRate = rate;
    }

    public void Run()
    {
        var sum = 0L;

        AnsiConsole
            .Status()
            .Start("Joltinizing...", ctx =>
            {
                var lines = InputReader.ReadLinesFromResource(_input);
                foreach (var line in lines)
                {
                    var numbers = line.Select(c =>  c - '0').ToArray();
                    var seedIndex = numbers.Length - 12;
                    var joltageIndices = Enumerable.Range(seedIndex, 12).ToList();

                    for (var i = 0; i < joltageIndices.Count; i++)
                    {
                        var stop = i == 0 ? 0 : joltageIndices[i - 1] + 1;
                        for(var j = joltageIndices[i]; j >= stop; j--)
                        {
                            if (numbers[joltageIndices[i]] <= numbers[j])
                            {
                                joltageIndices[i] = j;
                            }

                            PrintProgress(ctx, line, j, joltageIndices);
                            if (DoRefresh())
                            {
                                ctx.Refresh();
                                Thread.Sleep(100);
                            }
                        }

                        PrintProgress(ctx, line, i, joltageIndices);
                        if (DoRefresh())
                        {
                            ctx.Refresh();
                        }
                    }

                    var joltage = joltageIndices.Aggregate(0L, (acc, index) => acc * 10 + numbers[index]);
                    sum += joltage;

                    AnsiConsole.MarkupLine("Found joltage: {0}, sum: {1}", joltage, sum);

                }

                ctx.Status("Thinking some more");
            });

        PrintResult(sum);
    }

    private static void PrintProgress(StatusContext ctx, string line, int index, List<int> joltageIndices)
    {
        var formatted = new StringBuilder().Append("[green]");
        for (var i = 0; i < line.Length; i++)
        {
            var ch = line[i];
            var markup = ch.ToString();

            if (joltageIndices.Contains(i))
            {
                markup = $"[bold maroon]{markup}[/]";
            }

            if (i == index)
            {
                markup = $"[underline]{markup}[/]";
            }

            formatted.Append(markup);
        }

        ctx.Status(formatted.Append("[/]").ToString());
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