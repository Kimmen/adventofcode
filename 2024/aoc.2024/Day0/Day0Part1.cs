using Spectre.Console;

namespace Aoc.Day0;

public class Day0Part1 : IChallenge
{
    private string _input = "Aoc.Day0.input.txt";
    private long? _expectedResult;

    public void UseDevInput()
    {
        _input = "Aoc.Day0.input.dev.txt";
        _expectedResult = 24000L;
    }

    public void Run()
    {
        AnsiConsole.Status()
            .Start("Thinking...", ctx =>
            {
                ctx.Status("Reading file");
                var caloriesPerInventory = InputReader
                    .ReadLinesFromResource(_input)
                    .ChunkBy(string.IsNullOrWhiteSpace);

                Thread.Sleep(500);
                AnsiConsole.MarkupLine($"Number of inventories: [bold]{caloriesPerInventory.Count()}[/]");

                ctx.Status("Calculeting file");
                var totalCalories = caloriesPerInventory
                    .Select(x => x.Select(long.Parse).Sum())
                    .OrderDescending()
                    .Take(1)
                    .Sum();

                Thread.Sleep(500);
                var color = _expectedResult.HasValue ? "green" : "yellow";
                if (_expectedResult.HasValue && totalCalories != _expectedResult)
                {
                    color = "red";
                }

                AnsiConsole.MarkupLine($"Total calories: [{color} bold]{totalCalories}[/]");
            });
    }
}