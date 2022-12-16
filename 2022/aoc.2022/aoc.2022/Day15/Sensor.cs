using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc.Day15
{
    internal record struct Pos(long X, long Y);
    internal partial class Sensor
    {
        private static readonly Regex SensorBeaconRegex = SensorRegex();
        private readonly Pos left;
        private readonly Pos right;
        private readonly Pos up;
        private readonly Pos down;
        private readonly Pos center;
        private readonly Pos closestBeacon;

        public Sensor(Pos left, Pos right, Pos up, Pos down, Pos center, Pos closestBeacon)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
            
            this.center = center;
            this.closestBeacon = closestBeacon;
        }

        internal Pos ClosestBeacon => closestBeacon;

        internal Pos Center => center;

        internal bool Intersects(long row)
        {
            return row >= up.Y && row <= down.Y;
        }

        internal (long min, long max)? SensorAreaSlice(long row)
        {
            if (!Intersects(row))
            {
                return null;
            }

            var linesToCross = left.Y < row
               ? new[] { (left, down), (down, right) }
               : new[] { (left, up), (up, right) };

            var leftIntersection = GetIntersectionPoint(row, linesToCross.First());
            var rightIntersection = GetIntersectionPoint(row, linesToCross.Last());

            return (leftIntersection.X, rightIntersection.X);
        }

        internal IList<Pos> OverlappingPoints(long row)
        {
            if (!Intersects(row))
            {
                return new Pos[0]; 
            }

            var middle = left.Y < row
                ? down
                : up;

            var leftIntersection = left;
            var rightIntersection = right;

            Traverse(left, middle, p =>
            {
                if (p.Y == row)
                {
                    leftIntersection = p;
                    return false;
                }

                return true;
            });

            Traverse(middle, right, p =>
            {
                if (p.Y == row)
                {
                    rightIntersection = p;
                    return false;
                }

                return true;
            });

            var positions = new List<Pos>();
            Traverse(leftIntersection, rightIntersection, p =>
            {
                positions.Add(p);
                return true;
            });

            return positions;
        }

        private void Traverse(Pos start, Pos end, Func<Pos, bool> process)
        {
            var curr = start;
            if(!process(curr)) { return; }

            while (curr != end)
            {
                var dx = Math.Sign(end.X - curr.X);
                var dy = Math.Sign(end.Y - curr.Y);

                curr = new Pos(curr.X + dx, curr.Y + dy);

                if (!process(curr))
                {
                    return;
                }
            }
        }

        private Pos GetIntersectionPoint(long y, (Pos left, Pos right) shape)
        {
            const double factor = 10000D;
            //left 
            var row = (left: new Pos(shape.left.X - 1, y), right: new Pos(right.X + 1, y));

            // Determinant Method
            double x1_ = row.left.X / factor;
            double y1_ = row.left.Y / factor;
            double x2_ = row.right.X / factor;
            double y2_ = row.right.Y / factor;
            double x3_ = shape.left.X / factor;
            double y3_ = shape.left.Y / factor;
            double x4_ = shape.right.X / factor;
            double y4_ = shape.right.Y / factor;

            var ipX = ((x1_ * y2_ - y1_ * x2_) * (x3_ - x4_) - (x3_ * y4_ - y3_ * x4_) * (x1_ - x2_)) / (((x1_ - x2_) * (y3_ - y4_)) - ((y1_ - y2_) * (x3_ - x4_)));
            var ipY = ((x1_ * y2_ - y1_ * x2_) * (y3_ - y4_) - (x3_ * y4_ - y3_ * x4_) * (y1_ - y2_)) / (((x1_ - x2_) * (y3_ - y4_)) - ((y1_ - y2_) * (x3_ - x4_)));

            return new Pos(Convert.ToInt64(ipX * factor), Convert.ToInt64(ipY * factor));
        }
    }
}
