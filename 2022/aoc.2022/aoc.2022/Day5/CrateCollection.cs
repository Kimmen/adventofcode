using Aoc.Day5;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Day5
{
    public class CrateCollection
    {
        private List<Stack<string>> _stacks;
        private ICrateMover _mover;

        private CrateCollection(int stackCount, ICrateMover mover)
        {
            _stacks = Enumerable
                .Range(1, stackCount)
                .Select(x => new Stack<string>())
                .ToList();
            _mover = mover;
        }

        public void Move(MoveInstruction instruction)
        {
            _mover.Move(_stacks, instruction);
        }

        public string PeekAll()
        {
            return _stacks
                .Select(x => x.Peek())
                .Aggregate(string.Empty, (acc, next) => $"{acc}{next}");
        }

        public static CrateCollection Parse(IList<string> input, ICrateMover crateMover)
        {
            var stackInput = input
                .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                .Reverse() //start from the bottom
                .ToList();

            var stackConfig = stackInput.First();
            var stackCount = GetStackCount(stackConfig);
            var collection = new CrateCollection(stackCount, crateMover);

            foreach (var level in stackInput.Skip(1))
            {
                var creates = GetCreatesAtLevel(level);
                for (int i = 0; i < creates.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(creates[i]))
                    {
                        continue;
                    }

                    collection._stacks[i].Push(creates[i]);
                }
            }

            return collection;
        }

        private static List<string> GetCreatesAtLevel(string level)
        {
            return level
                        .Chunk(4)
                        .Select(x => new string(x))
                        .Select(x => x.Trim())
                        .Select(x => x.Replace("[", ""))
                        .Select(x => x.Replace("]", ""))
                        .ToList();
        }

        private static int GetStackCount(string stackConfig)
        {
            return int.Parse(stackConfig
                            .Replace(" ", "")
                            .Reverse()
                            .First()
                            .ToString());
        }
    }
}

