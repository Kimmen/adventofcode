using Spectre.Console;

namespace Aoc.Day7;

public partial class Part1 : IChallenge
{
    private string _input = "txt";
    private long? _expectedResult = 1507L;
    private long _refreshRate = 100;
    private long _refreshCount = 0L;

    public void UseDevInput()
    {
        _input = "dev.txt";
        _expectedResult = 21L;
    }

    public void RefreshRate(long rate)
    {
        _refreshRate = rate;
    }

    private string PuzzleInput => $"{GetType().Namespace}.input.{_input}";

    private const char Start = 'S';
    private const char Split = '^';
    private const char Beam = '|';

    public void Run()
    {
        var sum = 0L;
        var lines = InputReader.StreamLines(PuzzleInput);

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Manifolding...", ctx =>
            {
                using var lineEnumerator = lines.GetEnumerator();
                var lineNumber = 0;
                var beams = new HashSet<int>();
                
                while (lineEnumerator.MoveNext())
                {
                    var line =  lineEnumerator.Current.ToCharArray();
                    if (lineNumber == 0)
                    {
                        var start = GetStartPositionIndex(line);
                        beams.Add(start);
                    }
                    else if (lineNumber % 2 == 1)
                    {
                        foreach (var beamIndex in beams)
                        {
                            line[beamIndex] = Beam;
                        }
                    }
                    else
                    {
                        var splitters = GetSplitters(line);
                        foreach (var splitter in splitters)
                        {
                            if (!beams.Contains(splitter))
                            {
                                continue;
                            }
                            beams.Remove(splitter);
                            beams.Add(splitter - 1);
                            beams.Add(splitter + 1);
                            sum++;
                        }
                        
                        foreach (var beamIndex in beams.Where(b => b > -1 && b < line.Length))
                        {
                            line[beamIndex] = Beam;
                        }
                    }
                    
                    AnsiConsole.MarkupLine(new string(line));
                    ctx.Status($"There are {beams.Count} beams. Split sum is {sum}");
                    if (DoRefresh())
                    {
                        Thread.Sleep(100);
                    }
                    lineNumber++;
                }
                
            });
        
        PrintResult(sum);
    }

    private static ICollection<int> GetSplitters(ReadOnlySpan<char> line)
    {
        var indices = new List<int>();

        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == Split)
            {
                indices.Add(i);
            }
        }
        
        return indices;
    }

    private static int GetStartPositionIndex(ReadOnlySpan<char> line)
    {
        return line.IndexOf(Start);
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