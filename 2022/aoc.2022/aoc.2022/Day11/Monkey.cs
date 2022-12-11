using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc.Day11;

internal partial class Monkey
{
    public required int Index { get; set; }
    public required Queue<long> Items { get; set; }
    public required Func<long, long> Operation { get; set; }
    public required Func<long, int> Test { get; set; }




    [GeneratedRegex("(?'param1'(old|\\d+)) (?'op'[*, +]) (?'param2'(old|\\d+))", RegexOptions.Compiled)]
    private static partial Regex OpRegex();
    public static Monkey Parse(IEnumerable<string> monkeyLines)
    {
        var lines = monkeyLines.ToArray();
        var opRegex = OpRegex();

        //Ugly but probably works
        Func<long, long> GetOperation(string line)
        {
            var operationMatch = opRegex.Match(lines[2]);
            var param1 = operationMatch.Groups["param1"].Value;
            var param2 = operationMatch.Groups["param2"].Value;
            var op = operationMatch.Groups["op"].Value;

            return (old) =>
            {
                var p1 = param1 == "old" ? old : long.Parse(param1);
                var p2 = param2 == "old" ? old : long.Parse(param2);
                if (op == "+")
                {
                    return p1 + p2;
                }
                else
                {
                    return p1 * p2;
                }
            };
        }

        Func<long, int> GetTest(string line, string trueLine, string falseLine)
        {
            var divideBy = long.Parse(line[21..].Trim());
            var monkeyIndexTrue = int.Parse(trueLine[29..].Trim());
            var monkeyIndexFalse = int.Parse(falseLine[30..].Trim());

            return (x) => x % divideBy == 0
                ? monkeyIndexTrue
                : monkeyIndexFalse;
        }

        return new Monkey
        {
            Index = lines[0][7] - '0',
            Items = new Queue<long>(lines[1][18..].Split(',').Select(x => x.Trim()).Select(long.Parse)),
            Operation = GetOperation(lines[2]),
            Test = GetTest(lines[3], lines[4], lines[5])
        };

    }
}