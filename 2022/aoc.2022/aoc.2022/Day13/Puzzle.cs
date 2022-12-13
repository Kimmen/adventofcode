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
        public Queue<Packet> Packets { get; set; } = new Queue<Packet>();

        public void Add(Packet packet)
        {
            this.Packets.Enqueue(packet);
        }

        internal Packet? Next()
        {
            if(this.Packets.TryDequeue(out var packet)) return packet;
            return null;
        }
    }

    private int CalculatePackagesInRightOrder(string input)
    {
        var packetPairs = InputReader
            .ReadLinesFromResource(input)
            .ChunkBy(string.IsNullOrWhiteSpace)
            .Select(ParsePackets)
            .Select(IsInRightOrder)
            .Select(x => x!.Value)
            .ToList();

        return packetPairs
            .Select((x, i) => (Index: i + 1, IsInOrder: x))
            .Where(x => x.IsInOrder)
            .Sum(x => x.Index);
    }

    private bool? IsInRightOrder((ArrayPacket First, ArrayPacket Second) pair)
    {
        var (first, second) = pair;        
        var left = first.Next();
        var right = second.Next();

        while (left is not null && right is not null)
        {
            var leftValue = left as ValuePacket;
            var rightValue = right as ValuePacket;
            var leftArray = left as ArrayPacket;
            var rightArray = right as ArrayPacket;
            
            if (leftValue is not null && rightValue is not null
                && leftValue.Value != rightValue.Value)
            {
                return leftValue.Value < rightValue.Value;
            }

            if (leftArray is not null && rightValue is not null)
            {
                rightArray = new ArrayPacket();
                rightArray.Add(rightValue);
            } else if (rightArray is not null && leftValue is not null)
            {
                leftArray = new ArrayPacket();
                leftArray.Add(leftValue);
            }

            if (leftArray is not null && rightArray is not null)
            {
                var rightOrder = IsInRightOrder((leftArray, rightArray));
                if (rightOrder.HasValue)
                {
                    return rightOrder.Value;
                }
            }

            left = first.Next();
            right = second.Next();
        }

        if (left is null && right is not null)
        {
            return true;
        }
        else if (left is not null && right is null)
        {
            return false;
        }

        return null; //inconclusive;
    }

    private (ArrayPacket First, ArrayPacket Second) ParsePackets(IEnumerable<string> pairsInput)
    {

        return (ParsePacket(pairsInput.First()), ParsePacket(pairsInput.Last()));

    }

    private ArrayPacket ParsePacket(string input)
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

    private static ValuePacket ParseValue(string input, ref int i)
    {
        var current = input[i];
        var valueInput = new List<char>();
        while (char.IsDigit(current))
        {
            valueInput.Add(current);
            current = input[++i];
        }
        //unparse last char, let the main loop take care of it.
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
        Assert.Equal(13, result);
    }

    [Fact]
    public void Part1()
    {
        var result = CalculatePackagesInRightOrder("Aoc.Day13.input.txt");
        Assert.Equal(6420, result);
    }

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