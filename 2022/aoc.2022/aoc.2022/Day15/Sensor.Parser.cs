using System;
using System.Text.RegularExpressions;

namespace Aoc.Day15
{
    internal partial class Sensor
    {
        public static Sensor Parse(string line)
        {
            var match = SensorBeaconRegex.Match(line);

            if (!match.Success)
            {
                throw new ArgumentException(line, nameof(line));
            }

            var sx = int.Parse(match.Groups["sx"].Value);
            var sy = int.Parse(match.Groups["sy"].Value);
            var bx = int.Parse(match.Groups["bx"].Value);
            var by = int.Parse(match.Groups["by"].Value);

            var dx = Math.Abs(sx - bx);
            var dy = Math.Abs(sy - by);
            var length = dx + dy;

            return new Sensor(
                new Pos(sx - length, sy),
                new Pos(sx + length, sy),
                new Pos(sx, sy - length),
                new Pos(sx, sy + length),
                new Pos(sx, sy),
                new Pos(bx, by));
        }

        [GeneratedRegex("Sensor.+x=(?'sx'-?\\d+), y=(?'sy'(-?)\\d+).+beacon.+x=(?'bx'(-?)\\d+), y=(?'by'(-?)\\d+)")]
        private static partial Regex SensorRegex();
    }
}
