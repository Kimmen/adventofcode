using System;
using System.Text.RegularExpressions;

namespace Aoc.Day16
{
    internal partial class Valve
    {
        private static readonly Regex ValveRegex = ValveRegexFactory();

        public static Valve Parse(string line)
        {
            var match = ValveRegex.Match(line);
            if(!match.Success)
            {
                throw new InvalidOperationException();
            }

            var name = match.Groups["name"].Value;
            var rate = int.Parse(match.Groups["rate"].Value);
            var connectedValves = match.Groups["valves"].Value.Split(",", StringSplitOptions.TrimEntries);

            return new Valve(name, rate, connectedValves);
        }

        [GeneratedRegex("Valve (?'name'[A-Z]+) has flow rate=(?'rate'\\d+).+valve([s])? (?'valves'.+)$")]
        private static partial Regex ValveRegexFactory();
    }
}
