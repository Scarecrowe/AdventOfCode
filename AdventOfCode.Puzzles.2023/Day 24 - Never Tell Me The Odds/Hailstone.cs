namespace AdventOfCode.Puzzles._2023.Day_24___Never_Tell_Me_The_Odds
{
    public class Hailstone
    {
        public (long X, long Y, long Z) Point { get; private set; }

        public (long X, long Y, long Z) Velocity { get; private set; }

        public bool Infinity { get; private set; }

        public Hailstone((long X, long Y, long Z) point, (long X, long Y, long Z) velocity, bool infinity)
        {
            this.Point = point;
            this.Velocity = velocity;
            this.Infinity = infinity;
        }

        public bool IntersectsXY(Hailstone hailstone, long min, long max)
        {
            var (intersects, x, y, _) = this.IntersectsXY(hailstone);

            return intersects && x >= min && x <= max && y >= min && y <= max;
        }

        public double InterpolateZ(double t, long dvz) => Math.Round(this.Point.Z + t * (this.Velocity.Z + dvz), 3);

        public (bool intersects, double x, double y, double t) IsIntersectionXY(Hailstone hailstone, long dvy, long dvx)
            => hailstone.VOffsetXY(dvy, dvx).IntersectsXY(this.VOffsetXY(dvy, dvx));

        private Hailstone VOffsetXY(long dvy, long dvx)
            => new(this.Point, (this.Velocity.X + dvx, this.Velocity.Y + dvy, this.Velocity.Z), this.Infinity);

        private (bool, double x, double y, double t) IntersectsXY(Hailstone hailstone)
        {
            if (this.Velocity.X == 0 && hailstone.Velocity.X == 0)
            {
                return (false, 0, 0, -1);
            }

            return this.Infinity ? this.IntersectsInfinity(hailstone) : this.Intersects(hailstone);
        }

        private (bool, double x, double y, double t) Intersects(Hailstone hailstone)
        {
            bool currentVert = this.Velocity.X == 0;
            bool otherVert = hailstone.Velocity.X == 0;

            if (currentVert && otherVert)
            {
                return (false, 0, 0, -1);
            }

            double x, y, timeCurrent, timeOther;

            if (currentVert)
            {
                x = this.Point.X;

                double m2 = (double)hailstone.Velocity.Y / hailstone.Velocity.X;
                double c2 = hailstone.Point.Y - m2 * hailstone.Point.X;
                y = m2 * x + c2;

                timeCurrent = (y - this.Point.Y) / Velocity.Y;
                timeOther = (x - hailstone.Point.X) / hailstone.Velocity.X;
            }
            else if (otherVert)
            {
                x = hailstone.Point.X;

                double m1 = (double)Velocity.Y / Velocity.X;
                double c1 = Point.Y - m1 * Point.X;
                y = m1 * x + c1;

                timeCurrent = (x - Point.X) / Velocity.X;
                timeOther = (y - hailstone.Point.Y) / hailstone.Velocity.Y;
            }
            else
            {
                decimal m1 = Velocity.Y / (decimal)Velocity.X;
                decimal c1 = Point.Y - m1 * Point.X;

                decimal m2 = hailstone.Velocity.Y / (decimal)hailstone.Velocity.X;
                decimal c2 = hailstone.Point.Y - m2 * hailstone.Point.X;

                if (m1 == m2)
                {
                    return (false, 0, 0, -1);
                }

                decimal xd = (c2 - c1) / (m1 - m2);
                x = (double)xd;
                y = (double)(m1 * (xd - Point.X) + Point.Y);

                timeCurrent = (x - Point.X) / Velocity.X;
                timeOther = (x - hailstone.Point.X) / hailstone.Velocity.X;
            }

            if (timeCurrent < 0 || timeOther < 0)
            {
                return (false, 0, 0, -1);
            }

            return (true, Math.Round(x, 3), Math.Round(y, 3), timeCurrent);
        }

        private (bool, double x, double y, double t) IntersectsInfinity(Hailstone hailstone)
        {
            double m1 = Velocity.X == 0 ? double.PositiveInfinity : (double)Velocity.Y / Velocity.X;
            double m2 = hailstone.Velocity.X == 0 ? double.PositiveInfinity : (double)hailstone.Velocity.Y / hailstone.Velocity.X;

            double c1 = Point.Y - m1 * Point.X;
            double c2 = hailstone.Point.Y - m2 * hailstone.Point.X;

            if (m1 == m2)
            {
                return (false, 0, 0, -1);
            }

            double x = (c2 - c1) / (m1 - m2);
            double t1 = (x - Point.X) / Velocity.X;
            double t2 = (x - hailstone.Point.X) / hailstone.Velocity.X;

            if (t1 < 0 || t2 < 0)
            {
                return (false, 0, 0, -1);
            }

            double y = m1 * (x - Point.X) + Point.Y;

            return (true, Math.Round(x, 3), Math.Round(y, 3), t1);
        }
    }

}
