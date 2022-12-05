using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Aoc;

public static class InputReader
{
    public static IEnumerable<string> ReadLinesFromResource(string resourceName)
    {
        using var stream = typeof(InputReader).Assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);

        return reader.ReadToEnd().GetLines();
    }
    
    public static IEnumerable<string> GetLines(this string value)
    {
        return value.Split(Environment.NewLine);
    }
}