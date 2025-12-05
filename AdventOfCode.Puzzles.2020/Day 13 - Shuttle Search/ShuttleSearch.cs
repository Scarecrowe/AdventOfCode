namespace AdventOfCode.Puzzles._2020.Day_13___Shuttle_Search
{
    using AdventOfCode.Core.Extensions;

    public class ShuttleSearch
    {
        public static int EarliestBusId(string[] input)
        {
            int earliestDepature = input[0].ToInt();

            int[] busIds = input[1]
                .Split(",")
                .Where(x => x != "x")
                .ToInt();

            List<int> depatures = new();

            for (int i = 0; i < busIds.Length; i++)
            {
                int value = 0;

                while (value <= earliestDepature)
                {
                    value += busIds[i];
                }

                depatures.Add(value);
            }

            int min = depatures.Min();
            int index = depatures.IndexOf(min);

            int timeToWait = min - earliestDepature;

            return busIds[index] * timeToWait;
        }

        public static long EarliestTimestamp(string[] input)
        {
            List<long> busIds = input[1].Split(",")
                .Select(x => x == "x" ? 0 : x.ToLong())
                .ToList();

            Dictionary<long, int> departures = new();

            long time = busIds[0];
            long increment = busIds[0];

            for (int i = 1; i < busIds.Count; i++)
            {
                if (busIds[i] != 0)
                {
                    long value = busIds[i];

                    while ((time + busIds.IndexOf(value)) % value != 0)
                    {
                        time += increment;
                    }

                    increment *= value;
                }
            }

            return time;
        }
    }
}
