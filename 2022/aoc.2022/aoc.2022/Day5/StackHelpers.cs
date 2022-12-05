using System.Collections.Generic;

namespace Aoc.Day5;

public static class StackHelpers
{
    public static IEnumerable<T> PopMany<T>(this Stack<T> stack, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return stack.Pop();
        }
    }
}