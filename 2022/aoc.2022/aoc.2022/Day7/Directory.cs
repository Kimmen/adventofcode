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
    public Directory Root { get; set; } = null;
    public Directory Parent { get; set; } = null;
    public string Name { get; set; } = string.Empty;
    public int Size => Children.Sum(x => x.Size);
    public List<INode> Children { get; set; } = new();
}

public class File : INode
{
    public int Size { get; set; } = 0;
    public string Name { get; set; } = string.Empty;

}