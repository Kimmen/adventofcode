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
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd().GetLines();
    }
    
    public static IEnumerable<string> GetLines(this string value)
    {
        return value.Split(Environment.NewLine);
    }

    public static int ToNumeral(this BitArray binary)
    {
        if (binary == null)
            throw new ArgumentNullException("binary");
        if (binary.Length > 32)
            throw new ArgumentException("must be at most 32 bits long");

        var result = new int[1];
        binary.CopyTo(result, 0);
        return result[0];
    }
}