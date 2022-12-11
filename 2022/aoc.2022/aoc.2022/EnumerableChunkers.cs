using System.Collections.Generic;
using System;

namespace Aoc;

public static class EnumerableChunkers
{
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
}