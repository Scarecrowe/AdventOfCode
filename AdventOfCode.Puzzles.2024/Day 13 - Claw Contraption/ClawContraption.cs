namespace AdventOfCode.Puzzles._2024.Day_13___Claw_Contraption
{
    using System.Linq;

    public class ClawContraption(string[] input)
    {
        public long FewestTokens() => Calculate(0);

        public long FewestTokensWithOffset() => Calculate(10000000000000);

        private long Calculate(long offset)
        {
            long total = 0;
            int i = 0;

            while (i < input.Length)
            {
                var s = Parse(input[i++]);
                var x1 = s[0];
                var y1 = s[1];

                s = Parse(input[i++]);
                var x2 = s[0];
                var y2 = s[1];

                s = Parse(input[i++], isTarget: true);
                var x = s[0] + offset;
                var y = s[1] + offset;

                i++;

                var (A, B) = SolveLinearSystem(x1, y1, x2, y2, x, y);

                if (A >= 0 && B >= 0 && B * x2 + A * x1 == x && B * y2 + A * y1 == y)
                {
                    total += A * 3 + B;
                }
            }

            return total;
        }

        private static long[] Parse(string line, bool isTarget = false)
        {
            if (isTarget)
            {
                return line.Split(", ").Select(a => long.Parse(a.Split("=").Last())).ToArray();
            }
                
            return line.Split(", ").Select(a => long.Parse(a.Split("+").Last())).ToArray();
        }

        private static (long A, long B) SolveLinearSystem(long x1, long y1, long x2, long y2, long x, long y)
        {
            var b = (y * x1 - x * y1) / (y2 * x1 - x2 * y1);
            var a = (x - b * x2) / x1;

            return (a, b);
        }
    }
}
