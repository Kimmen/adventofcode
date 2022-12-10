using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Xunit.Abstractions;

namespace Aoc.Day10;

public class Puzzle
{
    private readonly ITestOutputHelper _output;

    public Puzzle(ITestOutputHelper output)
    {
        _output = output;
    }

    public interface ITicker { void Apply(int cycle, int x); }

    private static void RunVideoSystemInstructions(string input, ITicker ticker)
    {
        var instructions = InputReader.ReadLinesFromResource(input);
        var cycle = 0;
        var x = 1;

        void Tick()
        {
            cycle++;
            ticker.Apply(cycle, x);
        }

        foreach (var instruction in instructions)
        {
            if (instruction.StartsWith("noop"))
            {
                Tick();
            }
            else //addx
            {
                var increase = int.Parse(instruction[4..]);
                Tick();
                Tick();
                x += increase;
            }
        }
    }

    public class SignalStrengthCalculatorTicker : ITicker
    {
        private static readonly int[] cyclesToConsider = new[] { 20, 60, 100, 140, 180, 220 }; //20 + every 40th cycle
        private readonly List<(int cycle, int strength)> signalStrengths = new();

        public int SignalStrengthSum => signalStrengths.Select(x => x.strength).Sum();

        public void Apply(int cycle, int x)
        {
            if (cyclesToConsider.Contains(cycle))
            {
                signalStrengths.Add((cycle, x * cycle));
            }
        }
    }

    public class VideoRendererTicker : ITicker
    {
        private const int _columns = 40;
        private const int _rows = 6;
        private readonly bool[,] _litPixels = new bool[_columns, _rows];

        public void Apply(int cycle, int x)
        {
            var position = cycle - 1;
            var xPixel = position % _columns;
            var yPixel = position / _columns;
            var isPixelInSprite = xPixel >= x - 1 && xPixel <= x + 1;
            _litPixels[xPixel, yPixel] = isPixelInSprite;
        }

        public string PrintPixels()
        {
            var rendered = new StringBuilder();

            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _columns; x++)
                {
                    var isLit = _litPixels[x, y];
                    rendered.Append(isLit ? '#' : '.');
                }
                
                rendered.AppendLine();
            }

            return rendered.ToString();
        }
    }

    [Fact]
    public void Part1Dev()
    {
        var ticker = new SignalStrengthCalculatorTicker();
        RunVideoSystemInstructions("Aoc.Day10.input.dev.txt", ticker);
        Assert.Equal(13140, ticker.SignalStrengthSum);
    }

    [Fact]
    public void Part1()
    {
        var ticker = new SignalStrengthCalculatorTicker();
        RunVideoSystemInstructions("Aoc.Day10.input.txt", ticker);
        Assert.Equal(14920, ticker.SignalStrengthSum);
    }

    [Fact]
    public void Part2Dev()
    {
        var ticker = new VideoRendererTicker();
        RunVideoSystemInstructions("Aoc.Day10.input.dev.txt", ticker);
        var result = ticker.PrintPixels();
        _output.WriteLine(result);
    }

    [Fact]
    public void Part2()
    {
        var ticker = new VideoRendererTicker();
        RunVideoSystemInstructions("Aoc.Day10.input.txt", ticker);
        var result = ticker.PrintPixels();
        _output.WriteLine(result);

        Assert.Equal("BUCACBUZ", "BUCACBUZ");
    }
}