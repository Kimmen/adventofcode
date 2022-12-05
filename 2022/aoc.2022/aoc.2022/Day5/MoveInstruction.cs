using System;
using System.Text.RegularExpressions;

namespace Aoc.Day5
{
    internal partial class MoveInstruction
    {
        private static readonly Regex InputRegex = InputRegexBuilder();

        public MoveInstruction(int moveCount, int fromStack, int toStack)
        {
            MoveCount = moveCount;
            FromStack = fromStack;
            ToStack = toStack;
        }

        public int MoveCount { get; }
        public int FromStack { get; }
        public int ToStack { get; }

        internal static MoveInstruction Parse(string moveInput)
        {
            var match = InputRegex.Match(moveInput);
            if(!match.Success)
            {
                throw new InvalidOperationException("Invalid move instruction");    
            }

            var moveCount = int.Parse(match.Groups["move"].Value);
            var fromStackIndex = int.Parse(match.Groups["from"].Value) - 1;
            var toStackIndex = int.Parse(match.Groups["to"].Value) - 1;

            return new MoveInstruction(moveCount, fromStackIndex, toStackIndex);
        }

        [GeneratedRegex("^move (?'move'\\d+) from (?'from'\\d+) to (?'to'\\d+)$", RegexOptions.Compiled)]
        private static partial Regex InputRegexBuilder();
    }
}