using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc.Day7;

public class DirectoryBuilder
{
    public static Regex ChangeDirRegex = new("\\$ cd (?'name'.+)", RegexOptions.Compiled);
    public static Regex ListRegex = new("\\$ ls", RegexOptions.Compiled);
    public static Regex IsDirRegex = new("dir (?'name'.+)", RegexOptions.Compiled);
    public static Regex IsFileRegex = new("(?'size'\\d+) (?'name'.+)", RegexOptions.Compiled);
    public static Directory Build(IEnumerable<string> lines)
    {
        var root = new Directory {Name = "/"};
        root.Root = root;
        
        var current = root;
        foreach (var line in lines)
        {
            if (ChangeDirRegex.IsMatch(line))
            {
                var match = ChangeDirRegex.Match(line);
                current = ChangeDirectory(current, match.Groups["name"].Value);
            }
            else if (ListRegex.IsMatch(line))
            {
                //What
            }
            else if (IsDirRegex.IsMatch(line))
            {
                var match = IsDirRegex.Match(line);
                var directory = new Directory
                {
                    Name = match.Groups["name"].Value,
                    Parent = current,
                    Root = current.Root
                };
                current.Children.Add(directory);
            }
            else if (IsFileRegex.IsMatch(line))
            {
                var match = IsFileRegex.Match(line);
                var directory = new File
                {
                    Name = match.Groups["name"].Value,
                    Size = int.Parse(match.Groups["size"].Value)
                };
                current.Children.Add(directory);
            }
        }

        return root;
    }

    private static Directory ChangeDirectory(Directory current, string name)
    {
        if (name == "/")
        {
            return current.Name == "/" ? current : current.Root;
        }

        if (name == "..")
        {
            return current.Parent;
        }

        var childDirectory = current.Children
            .Where(x => x is Directory)
            .Cast<Directory>()
            .FirstOrDefault(x => x.Name == name);

        if (childDirectory is not null)
        {
            return childDirectory;
        }
        
        childDirectory = new Directory
        {
            Name = name,
            Parent = current,
            Root = current.Root
        };
        current.Children.Add(childDirectory);

        return childDirectory;
    }
}