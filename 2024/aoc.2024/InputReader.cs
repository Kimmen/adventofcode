namespace Aoc;

public static class InputReader
{
    public static IEnumerable<string> ReadLinesFromResource(string resourceName)
    {
        return ReadContentFromResource(resourceName).GetLines();
    }

    public static string ReadContentFromResource(string resourceName)
    {
        using var stream = typeof(InputReader).Assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);

        return reader.ReadToEnd();
    }

    public static IEnumerable<string> GetLines(this string value)
    {
        return value.Split(Environment.NewLine);
    }

    public static IEnumerable<string> StreamLines(string resourceName)
    {
        using var stream = typeof(InputReader).Assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        while (!reader.EndOfStream)
        {
            yield return reader.ReadLine();
        }
    }
}