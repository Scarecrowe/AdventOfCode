namespace AdventOfCode.Puzzles._2023.Day_24___Never_Tell_Me_The_Odds
{
    public class NeverTellMeTheOdds
    {
        private Hailstone[] Hailstones { get; set; }

        public NeverTellMeTheOdds(string[] input, bool infinity = true)
        {
            this.Hailstones = this.ParseHailstones(input, infinity);
        }

        public Hailstone[] ParseHailstones(string[] input, bool infinity)
        {
            return input.Select(line =>
            {
                string[] tokens = line.Split(" @ ");
                long[] point = tokens[0].Split(", ").Select(long.Parse).ToArray();
                long[] velocity = tokens[1].Split(", ").Select(long.Parse).ToArray();

                return new Hailstone((point[0], point[1], point[2]), (velocity[0], velocity[1], velocity[2]), infinity);
            }).ToArray();
        }

        public int TestAreaIntersections(long min = 200000000000000, long max = 400000000000000) => CountIntersectionsXY(this.Hailstones, min, max);

        public long SingleThrow() => SingleThrowPosition(this.Hailstones);

        private static int CountIntersectionsXY(Hailstone[] hailstones, long min, long max)
            => hailstones
                .SelectMany((h, i) => hailstones.Skip(i + 1), (h1, h2) => new { h1, h2 })
                .Count(p => p.h1.IntersectsXY(p.h2, min, max));

        public static long SingleThrowPosition(Hailstone[] hailstones)
        {
            var (hs0, hs1, hs2, hs3) = (hailstones[0], hailstones[1], hailstones[^2], hailstones[^1]);

            foreach (int y in SearchSpace())
            {
                foreach (int x in SearchSpace())
                {
                    var i1 = hs0.IsIntersectionXY(hs1, y, x);
                    var i2 = hs0.IsIntersectionXY(hs2, y, x);
                    var i3 = hs0.IsIntersectionXY(hs3, y, x);

                    if (!i1.intersects || !i2.intersects || !i3.intersects)
                    {
                        continue;
                    }

                    if ((i1.y, i1.x) != (i2.y, i2.x) || (i1.y, i1.x) != (i3.y, i3.x))
                    {
                        continue;
                    }

                    foreach (int z in SearchSpace())
                    {
                        var z1 = hs1.InterpolateZ(i1.t, z);
                        var z2 = hs2.InterpolateZ(i2.t, z);
                        var z3 = hs3.InterpolateZ(i3.t, z);

                        if (z1 == z2 && z1 == z3)
                        {
                            return (long)(i1.x + i1.y + z1);
                        }
                    }
                }
            }

            throw new Exception("No intersection found");

            static IEnumerable<int> SearchSpace() => Enumerable.Range(-300, 600);
        }
    }
}
