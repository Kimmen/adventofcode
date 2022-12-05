using System;

namespace Aoc.Day4
{
    internal static class RangHelpers
    {
        public static bool Contains(this Range range, Range other)
        {
            return range.Start.Value <= other.Start.Value && range.End.Value >= other.End.Value;
        }

        public static bool Contains(this Range range, Index index)
        {
            return range.Start.Value <= index.Value && range.End.Value >= index.Value;
        }

        public static bool Overlaps(this Range range, Range other)
        {
            return range.Contains(other.Start) || range.Contains(other.End) ||
                other.Contains(range.Start) || other.Contains(range.End);
        }
    }
}