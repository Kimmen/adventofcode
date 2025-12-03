using System.Text.RegularExpressions;
using Spectre.Console;

namespace Aoc.Day3;

public partial class Part1 : IChallenge
{
    private string _input = "Aoc.Day3.input.txt";
    private long? _expectedResult = 21139440284L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;

    public void UseDevInput()
    {
        _input = "Aoc.Day3.input.dev.txt";
        _expectedResult = 357L;
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

                    var start = 0;
                    var end = 1;
                    var index = 0;

                    while (index < numbers.Length - 1)
                    {
                        var startValue = numbers[start];
                        var endValue = numbers[end];
                        var currentValue = numbers[index];
                        var nextValue = numbers[index + 1];

                        if (startValue < currentValue)
                        {
                            start = index;
                        }

                        if(endValue < nextValue)
                        {
                            end = index + 1;
                        }

                        if (end == start)
                        {
                            end++;
                        }

                        PrintProgress(ctx, line, index, start, end);
                        if (DoRefresh())
                        {
                            ctx.Refresh();
                            Thread.Sleep(100);
                        }

                        index++;
                    }
                    sum+= numbers[start] * 10 + numbers[end];
                    AnsiConsole.MarkupLine("Found joltage: {0}{1}, sum: {2}", numbers[start], numbers[end], sum);

                }

                ctx.Status("Thinking some more");
            });

        PrintResult(sum);
    }

    private static void PrintProgress(StatusContext ctx, string line, int index, int start, int end)
    {
        var formatted = "";
        for (var i = 0; i < line.Length; i++)
        {
            var ch = line[i];
            var markup = ch.ToString();

            if (i == start || i == end)
                markup = $"[bold]{markup}[/]";
            if (i == index)
                markup = $"[underline]{markup}[/]";

            formatted += markup;
        }
        ctx.Status(formatted);
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