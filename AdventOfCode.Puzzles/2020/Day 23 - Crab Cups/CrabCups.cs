namespace AdventOfCode.Puzzles._2020.Day_23___Crab_Cups
{
    using AdventOfCode.Core.Extensions;

    public class CrabCups
    {
        public static string Short(string[] input)
        {
            (Cup, Cup?, Dictionary<long, Cup>) cups = BuildCups(input[0]);

            Play(cups.Item1, 100, cups.Item3, 9);

            return cups.Item2?.ToString() ?? string.Empty;
        }

        public static long Long(string[] input)
        {
            (Cup, Cup?, Dictionary<long, Cup>) cups = BuildCups(input[0], 1000000 - input[0].Length);

            Play(cups.Item1, 10000000, cups.Item3, 1000000);

            return (cups.Item2?.Next?.Value ?? 0) * (cups.Item2?.Next?.Next?.Value ?? 0);
        }

        private static (Cup, Cup?, Dictionary<long, Cup>) BuildCups(string input, int extras = 0)
        {
            Dictionary<long, Cup> map = new();
            char[] chars = input.ToCharArray();
            Cup start = new();
            start.SetValue(chars[0].ToString().ToInt());
            map.Add(start.Value, start);

            Cup current = start;
            Cup? cupOne = null;

            for (int i = 1; i < chars.Length; i++)
            {
                Cup cup = new Cup().SetValue(chars[i].ToString().ToInt());

                map.Add(cup.Value, cup);

                if (cup.Value == 1)
                {
                    cupOne = cup;
                }

                current = current.SetNext(cup);
            }

            var value = 10;

            for (int i = 0; i < extras; i++)
            {
                Cup cup = new Cup().SetValue(value);
                map.Add(cup.Value, cup);
                current = current.SetNext(cup);
                value++;
            }

            current.SetNext(start);

            return (start, cupOne, map);
        }

        private static void Play(Cup? current, int moves, Dictionary<long, Cup> map, int max)
        {
            for (int i = 0; i < moves; i++)
            {
                Cup? start = current?.Next;
                Cup? end = start?.Next?.Next;
                HashSet<long> value = new()
                {
                    start?.Value ?? 0,
                    start?.Next?.Value ?? 0,
                    start?.Next?.Next?.Value ?? 0,
                };

                current?.SetNext(end?.Next ?? new());

                long next = current?.Value ?? 0;
                do
                {
                    next--;
                    next = next == 0 ? max : next;
                }
                while (value.Contains(next));

                Cup destination = map[next];
                Cup? nextCup = destination?.Next;
                destination?.SetNext(start ?? new());
                end?.SetNext(nextCup ?? new());

                current = current?.Next;
            }
        }
    }
}
