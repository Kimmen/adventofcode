using System.Data;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace Aoc.Day7;

public partial class Part2 : IChallenge
{
    private string _inputExt = "txt";

    private long? _expectedResult ;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 11387;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var lines = InputReader.StreamLines(PuzzleInput);

        var sum = 0L;
        AnsiConsole.Status()
            .Start("Calculating... ", ctx =>
            {
                foreach (var line in lines)
                {
                    AnsiConsole.MarkupLine(line);
                    var numbers = MathOpRegex().Matches(line).Select(m => m.Value).ToArray();
                    var target = long.Parse(numbers[0]);
                    var terms = numbers.Skip(1).Select(long.Parse).ToArray();

                    var isCorrect = CalculateTerms(terms, 0, target);

                    if (isCorrect)
                    {
                        sum += target;
                        ctx.Status($"Calculating... {sum}");
                    }
                }
            });


        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Sum: ");
        PrintResult(sum);
    }

    private bool CalculateTerms(long[] terms, int termIndex, long target)
    {
        var result = PerformInitial(terms, termIndex);
        return CalculateTerms(terms, termIndex + 1, target, result);
    }

    private static bool CalculateTerms(long[] terms, int termIndex, long target, long result)
    {
        var padding = GetPadding(termIndex);
        if (result > target)
        {
            AnsiConsole.MarkupInterpolated($"{padding} = [deeppink4_1]{result}[/]");
            AnsiConsole.MarkupLine(", [maroon]Exceeded target prematurely[/]");
            return false;
        }

        if (termIndex == terms.Length)
        {
            var isCorrect = result == target;
            AnsiConsole.MarkupInterpolated($"{padding} = [deeppink4_1]{result}[/]");
            AnsiConsole.MarkupLine(isCorrect ? ", [darkolivegreen3_1]Target reached[/]" : ", [maroon]Failed to reach target[/]");

            return isCorrect;
        }

        return CalculateTerms(terms, termIndex + 1, target, PerformAddition(terms, termIndex, result))
            || CalculateTerms(terms, termIndex + 1, target, PerformMultiplication(terms, termIndex, result))
            || CalculateTerms(terms, termIndex + 1, target, PerformConcat(terms, termIndex, result));
    }

    private long PerformInitial(long[] terms, int termIndex)
    {
        var term = terms[termIndex];
        AnsiConsole.MarkupLine($"{GetPadding(termIndex)}{term}");
        return terms[termIndex];
    }


    private static long PerformAddition(long[] terms, int termIndex, long? result)
    {
        var term = terms[termIndex];
        AnsiConsole.MarkupLine($"{GetPadding(termIndex)}[darkorange3]{RenderOperation(Operation.Add)}[/] {term}");
        return (result ?? 0) + term;
    }

    private static long PerformMultiplication(long[] terms, int termIndex, long? result)
    {
        var term = terms[termIndex];
        AnsiConsole.MarkupLine($"{GetPadding(termIndex)}[darkolivegreen3_1]{RenderOperation(Operation.Mul)}[/] {term}");
        return (result ?? 1) * term;
    }

    private static long PerformConcat(long[] terms, int termIndex, long? result)
    {
        var term = terms[termIndex];
        AnsiConsole.MarkupLine($"{GetPadding(termIndex)}[darkkhaki]{RenderOperation(Operation.Concat)}[/] {term}");
        return result.HasValue
            ? result.Value * (long)Math.Pow(10, (int)Math.Log10(term) + 1) + term
            : term;
    }

    private static string GetPadding(int termIndex)
    {
        return new string(' ', termIndex * 2);
    }

    private static string RenderOperation(Operation? op)
    {
        return op switch
        {
            Operation.Add => "+",
            Operation.Mul => "*",
            Operation.Concat => "||",
            _ => ""
        };
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

    enum Operation
    {
        Add,
        Mul,
        Concat
    }

    [GeneratedRegex(@"(?:\d+)+", RegexOptions.Compiled)]
    private static partial Regex MathOpRegex();
}