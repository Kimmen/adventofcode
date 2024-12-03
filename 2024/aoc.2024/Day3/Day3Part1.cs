using System.Text.RegularExpressions;
using Spectre.Console;

namespace Aoc.Day3;

public partial class Day3Part1 : IChallenge
{
    private string _inputExt = "txt";
    private long? _expectedResult = null;

    public void UseDevInput()
    {
        _inputExt = "dev.txt";
        _expectedResult = 161;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_inputExt}";

    public void Run()
    {
        var input = InputReader.ReadContentFromResource(PuzzleInput);

        var matcher = MulRegex();
        var matches = matcher.Matches(input).ToDictionary(x => x.Index, x => x);

        var commonStyle = Style.Plain.Foreground(Color.LightSkyBlue3_1);
        var matchStyle = Style.Plain.Foreground(Color.DeepSkyBlue4_1).Decoration(Decoration.Bold);

        var products = new List<(string Value, long Product)>();
        // out of match range by default
        var matchRange = Array.Empty<int>();
        for (var i = 0; i < input.Length; i++)
        {
            var character = input[i].ToString().EscapeMarkup();
            var match = matches.GetValueOrDefault(i);

            if(match is not null)
            {
                var product = CalculateProduct(match);
                matchRange = Enumerable.Range(match.Index, match.Length).ToArray();
                products.Add((match.Value, product));
            }

            var style = matchRange.Contains(i)  ? matchStyle : commonStyle;
            AnsiConsole.Write(new Text(character, style));
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
}