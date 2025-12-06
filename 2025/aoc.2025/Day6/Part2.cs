using Spectre.Console;
using Spectre.Console.Rendering;

namespace Aoc.Day6;

public partial class Part2 : IChallenge
{
    private string _input = "txt";
    private long? _expectedResult = 5667835681547L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;
    
    const int ColumnsInViewPort = 10;

    public void UseDevInput()
    {
        _input = "dev.txt";
        _expectedResult = 3263827L;
    }

    public void RefreshRate(long rate)
    {
        _refreshRate = rate;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_input}";

    public void Run()
    {
        var sum = 0L;

        var lines = InputReader.ReadLinesFromResource(PuzzleInput);
        var operatorsLine = lines
            .Last()
            //add extra space to make the TakeWhile(isWhitespace) also include last char in line
            .Append(' ')
            .ToArray();
        
        var values = lines
            .Take(lines.Count - 1)
            .ToArray();  

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Calculating...", ctx =>
            {
                var currentIndex = 0;

                while (currentIndex < operatorsLine.Length)
                {
                    var op = operatorsLine[currentIndex];
                    var spacesToNextOp = operatorsLine
                        .Skip(currentIndex + 1)
                        .TakeWhile(char.IsWhiteSpace)
                        .Count();
                    
                    var numbers = Enumerable
                        .Range(currentIndex, spacesToNextOp)
                        .Select(i => GetNumberForColumn(values, i))
                        .ToArray();
                    
                    var total = op switch
                    {
                        '*' => numbers.Aggregate((total, n) => total * n),
                        '+' => numbers.Aggregate((total, n) => total + n),
                        _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
                    };
                    
                    sum += total;
                    AnsiConsole.MarkupLineInterpolated($"Total is {total}, running sum is {sum}");
                    currentIndex = spacesToNextOp + currentIndex + 1;
                    
                    if (DoRefresh())
                    {
                        Thread.Sleep(300);
                    }
                }

                //
                // for (var c = 0; c < columns; c++)
                // {
                //     var numbers = GetNumberForColumn(valuesGrid, c);
                //
                //     var total = op switch
                //     {
                //         '*' => numbers.Aggregate((total, n) => total * n),
                //         '+' => numbers.Aggregate((total, n) => total + n),
                //         _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
                //     };
                //
                //     sum += total;
                //     AnsiConsole.MarkupLineInterpolated($"Total is {total}, running sum is {sum}");
                //     if (DoRefresh())
                //     {
                //         Thread.Sleep(300);
                //     }
                // }
            });
        
        PrintResult(sum);
    }

    private static long GetNumberForColumn(string[] values, int i)
    {
        var digits = new List<char>();
        foreach (var l in values)
        {
            var c = l[i];
            if (char.IsDigit(l[i]))
            {
                digits.Add(l[i]);
            }
        }
        
        return long.Parse(string.Join("", digits));
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