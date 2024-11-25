using Spectre.Console;

namespace AdventOfCode.Tweventyfour.Day0;

public class Day0Part1 : IChallenge
{
    public async Task Run()
    {
        await AnsiConsole.Status()
            .StartAsync("Thinking...", async ctx =>
            {
                AnsiConsole.MarkupLine("Doing some work...");
                Thread.Sleep(1000);

                // Update the status and spinner
                ctx.Status("Thinking some more");
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("green"));

                // Simulate some work
                AnsiConsole.MarkupLine("Doing some more work...");
                Thread.Sleep(2000);
            });
    }
}