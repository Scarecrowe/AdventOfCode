namespace AdventOfCode.Puzzles._2022.Day_25___Full_of_Hot_Air
{
    using AdventOfCode.Core.Extensions;

    public class FullOfHotAir
    {
        private static Dictionary<char, long> Map { get; } = new() { { '0', 0 }, { '1', 1 }, { '2', 2 }, { '=', -2 }, { '-', -1 } };

        private static List<char> Digits { get; } = new() { '0', '1', '2', '=', '-' };

        public static string Snafu(string[] input)
        {
            long sum = input.Sum(x => ToInt(x));

            int log = (int)Math.Ceiling(Math.Log(sum, 5));
            char[] value = "=".PadRight(log, '=').ToArray();

            while (true)
            {
                long val = ToInt(value.Join());

                if (val == sum)
                {
                    return value.Join();
                }

                long diff = (sum - val).Abs();
                int nlog = Math.Max((int)Math.Ceiling(Math.Log(diff, 5)), 1);
                value[log - nlog] = Digits[(Digits.IndexOf(value[log - nlog]) + 1) % 5];
            }

            throw new InvalidOperationException();
        }

        private static long ToInt(string value)
        {
            long result = 0;
            long pow = 1;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                result += Map[value[i]] * pow;
                pow *= 5;
            }

            return result;
        }
    }
}
