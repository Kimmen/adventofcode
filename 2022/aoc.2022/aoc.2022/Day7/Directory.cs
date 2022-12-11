using System.Collections.Generic;
using System.Linq;

namespace Aoc.Day7;

public interface INode
{
    public string Name { get; }
    public int Size { get; }
}

public partial class Directory : INode
{
    public Directory Root { get; set; }
    public Directory Parent { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Size => Children.Sum(x => x.Size);
    public List<INode> Children { get; set; } = new();
    
    public IEnumerable<Directory> FlattenDirectories()
    {
        yield return this;
      
        foreach (var child in this.Children)
        {
            if (child is not Directory d)
            {
                continue;
            }
            
            var matchingChildren = d.FlattenDirectories();

            foreach (var matchingChild in matchingChildren)
            {
                yield return matchingChild;
            }
        }
    }
}

public class File : INode
{
    public int Size { get; set; } = 0;
    public string Name { get; set; } = string.Empty;

}