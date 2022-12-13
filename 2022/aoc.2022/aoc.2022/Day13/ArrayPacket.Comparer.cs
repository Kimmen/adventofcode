using System;
using System.Collections.Generic;

namespace Aoc.Day13;

public class ArrayPacketComparer : IComparer<ArrayPacket>
{
    public int Compare(ArrayPacket? x, ArrayPacket? y)
    {
        if(x == null || y == null) return 0;
        
        return x?.CompareTo(y) ?? 1;
    }
}

public partial class ArrayPacket : IComparable<ArrayPacket>
{
    public int CompareTo(ArrayPacket? other)
    {
        this.ResetNext();
        other?.ResetNext();

        var left = this.Next();
        var right = other?.Next();

        while (left is not null && right is not null)
        {
            var leftValue = left as ValuePacket;
            var rightValue = right as ValuePacket;
            var leftArray = left as ArrayPacket;
            var rightArray = right as ArrayPacket;

            if (leftValue is not null && rightValue is not null
                && leftValue.Value != rightValue.Value)
            {
                return leftValue.Value.CompareTo(rightValue.Value);
            }

            if (leftArray is not null && rightValue is not null)
            {
                rightArray = new ArrayPacket();
                rightArray.Add(rightValue);
            }
            else if (rightArray is not null && leftValue is not null)
            {
                leftArray = new ArrayPacket();
                leftArray.Add(leftValue);
            }

            if (leftArray is not null && rightArray is not null)
            {
                var comparator = leftArray.CompareTo(rightArray);

                if (comparator != 0)
                {
                    return comparator;
                }
            }

            left = this.Next();
            right = other?.Next();
        }

        if (left is null && right is not null)
        {
            return -1;
        }
        else if (left is not null && right is null)
        {
            return 1;
        }

        return 0;
    }
}