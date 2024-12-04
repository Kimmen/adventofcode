using System.Text.RegularExpressions;
using Spectre.Console;

namespace Aoc.Day3;

public partial class Part2 : IChallenge
{
    private string _inputExt = "txt";
    private long? _expectedResult = null;

    public void UseDevInput()
    {
        _inputExt = "dev2.txt";
        _expectedResult = 48;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var input = InputReader.ReadContentFromResource(PuzzleInput);

        var mulMatcher = MulRegex();
        var doMatcher = DoRegex();
        var dontMatcher = DontRegex();

        var multipliers = mulMatcher.Matches(input).ToDictionary(x => x.Index, x => x);
        var enablers = doMatcher.Matches(input).ToDictionary(x => x.Index, x => x);
        var disablers = dontMatcher.Matches(input).ToDictionary(x => x.Index, x => x);

        var commonStyle = Style.Plain.Foreground(Color.LightSkyBlue3_1);
        var multiplierStyle = Style.Plain.Foreground(Color.DeepSkyBlue4_1).Decoration(Decoration.Bold);
        var doStyle = Style.Plain.Foreground(Color.SpringGreen4).Decoration(Decoration.Bold);
        var dontStyle = Style.Plain.Foreground(Color.Maroon).Decoration(Decoration.Bold);

        var products = new List<(string Value, long Product)>();
        var keywordRange = Array.Empty<int>();
        var keywordStyle = commonStyle;
        var isEnabled = true;

        for (var i = 0; i < input.Length; i++)
        {
            var match = multipliers.GetValueOrDefault(i);
            var enableMatch = enablers.GetValueOrDefault(i);
            var disableMatch = disablers.GetValueOrDefault(i);

            if(enableMatch is not null)
            {
                isEnabled = true;

                keywordRange = Enumerable.Range(enableMatch.Index, enableMatch.Length).ToArray();
                keywordStyle = doStyle;

            }
            if(disableMatch is not null)
            {
                isEnabled = false;

                keywordRange = Enumerable.Range(disableMatch.Index, disableMatch.Length).ToArray();
                keywordStyle = dontStyle;
            }

            if(isEnabled && match is not null)
            {
                var product = CalculateProduct(match);
                products.Add((match.Value, product));

                keywordRange = Enumerable.Range(match.Index, match.Length).ToArray();
                keywordStyle = multiplierStyle;
            }

            var character = input[i].ToString().EscapeMarkup();
            AnsiConsole.Write(new Text(character, keywordRange.Contains(i) ? keywordStyle : commonStyle));
        }
        AnsiConsole.MarkupLine("");

        AnsiConsole.MarkupLine("Product: ");
        PrintResult(products.Sum(x => x.Product));
    }

    private long CalculateProduct(Match match)
    {
        var firstNumber = long.Parse(match.Groups[1].Value);
        var secondNumber = long.Parse(match.Groups[2].Value);
        return firstNumber * secondNumber;
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

    [GeneratedRegex(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled)]
    private static partial Regex MulRegex();
    [GeneratedRegex(@"do\(\)", RegexOptions.Compiled)]
    private static partial Regex DoRegex();
    [GeneratedRegex(@"don't\(\)", RegexOptions.Compiled)]
    private static partial Regex DontRegex();
}