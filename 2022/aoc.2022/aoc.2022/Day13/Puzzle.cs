using FluentAssertions;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Xunit;

namespace Aoc.Day13;

public class Puzzle
{

    public abstract class Packet
    {
        
    }
    public sealed class ValuePacket : Packet
    {
        public int Value { get; set; }
    }
    public sealed class ArrayPacket : Packet
    {
        public ArrayPacket? Parent { get; set; }
        public List<Packet> Packets { get; set; } = new List<Packet>();

        public void Add(Packet packet)
        {
            this.Packets.Add(packet);
        }
    }

    private int CalculatePackagesInRightOrder(string input)
    {
        var packatPairs = InputReader
            .ReadLinesFromResource(input)
            .ChunkBy(string.IsNullOrWhiteSpace)
            .Select(ParsePackets)
            .ToList();

        return 0;
    }

    private (Packet First, Packet Last) ParsePackets(IEnumerable<string> pairsInput)
    {

        return (ParsePacket(pairsInput.First()), ParsePacket(pairsInput.Last()));

    }

    private Packet ParsePacket(string input)
    {
        var i = 1;
        var currentArray = new ArrayPacket();
        while (i < input.Length)
        {
            var current = input[i];

            if (current == '[')
            {
                var newArray = new ArrayPacket
                {
                    Parent = currentArray
                };
                currentArray.Add(newArray);
                currentArray = newArray;
                
            }
            else if (current == ']')
            {
                currentArray = currentArray.Parent ?? currentArray;
            }
            else if (char.IsDigit(current))
            {
                var valuePacket = ParseValue(input, ref i);
                currentArray.Add(valuePacket);
            }

            i++;
        }

        return currentArray;
    }

    private ValuePacket ParseValue(string input, ref int i)
    {
        var current = input[i];
        var valueInput = new List<char>();
        while (char.IsDigit(current))
        {
            valueInput.Add(current);
            current = input[++i];
        }
        //go back a step
        i--;

        return new ValuePacket
        {
            Value = int.Parse(valueInput.ToArray()),    
        };
    }

    [Fact]
    public void Part1Dev()
    {
        var result = CalculatePackagesInRightOrder("Aoc.Day13.input.dev.txt");
        Assert.Equal(12, result);
    }

    //[Fact]
    //public void Part1()
    //{
    //    var shortestPath = DetermineShortestHike("Aoc.Day12.input.txt", HikeOnlyFromStart);
    //    Assert.Equal(391, shortestPath);
    //}

    //[Fact]
    //public void Part2Dev()
    //{
    //    var shortestPath = DetermineShortestHike("Aoc.Day12.input.dev.txt", HikeFromAllLowest);
    //    Assert.Equal(29, shortestPath);
    //}

    //[Fact]
    //public void Part2()
    //{
    //    var shortestPath = DetermineShortestHike("Aoc.Day12.input.txt", HikeFromAllLowest);
    //    Assert.Equal(386, shortestPath);
    //}
}