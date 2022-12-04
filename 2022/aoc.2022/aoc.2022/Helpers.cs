using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Aoc;

public static class Helpers
{
    public static IEnumerable<string> ReadLinesFromResource(string resourceName)
    {
        using var stream = typeof(Helpers).Assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);

        return reader.ReadToEnd().GetLines();
    }
    
    public static IEnumerable<string> GetLines(this string value)
    {
        return value.Split(Environment.NewLine);
    }

    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, Predicate<T> splitPredicate)
    {
        IEnumerable<T> GetChunk(IEnumerator<T> enumerator, Predicate<T> predicate)
        {
            do
            {
                if (predicate(enumerator.Current))
                {
                    yield break;
                }

                yield return enumerator.Current;
            } while (enumerator.MoveNext());

        }

        var sourceEnumerator = source.GetEnumerator();
        while (sourceEnumerator.MoveNext())
        {
            yield return GetChunk(sourceEnumerator, splitPredicate);
        }
    }

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