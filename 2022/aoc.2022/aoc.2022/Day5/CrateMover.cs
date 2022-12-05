using Aoc;
using Aoc.Day5;

using System.Collections.Generic;
using System.Linq;

public interface ICrateMover
{
    void Move(List<Stack<string>> stacks, MoveInstruction instruction);
}

public class SingleCrateMover : ICrateMover
{
    public void Move(List<Stack<string>> stacks, MoveInstruction instruction)
    {
        for (int i = 0; i < instruction.MoveCount; i++)
        {
            var crate = stacks[instruction.FromStack].Pop();
            stacks[instruction.ToStack].Push(crate);
        }
    }
}

public class MultipleCrateMover : ICrateMover
{
    public void Move(List<Stack<string>> stacks, MoveInstruction instruction)
    {
        var crates = stacks[instruction.FromStack]
            .PopMany(instruction.MoveCount)
            .Reverse();

        foreach (var crate in crates)
        {
            stacks[instruction.ToStack].Push(crate);
        }
    }
}