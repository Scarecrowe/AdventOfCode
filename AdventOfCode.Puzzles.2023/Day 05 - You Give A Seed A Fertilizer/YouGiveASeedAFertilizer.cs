namespace AdventOfCode.Puzzles._2023.Day_05___You_Give_A_Seed_A_Fertilizer
{
    public class YouGiveASeedAFertilizer
    {
        public YouGiveASeedAFertilizer(List<string> input)
        {
            this.Seeds = new();
            this.Almanac = this.Parse(input);
            this.Times = new();
        }

        private List<long> Seeds { get; set; }

        private List<List<long[]>> Almanac { get; }

        private List<double> Times { get; }

        public long LowestSeed()
        {
            long result = long.MaxValue;

            foreach(long seed in this.Seeds)
            {
                long location = this.SeedToLocation(seed);

                if (location < result)
                {
                    result = location;
                }
            }

            return result;
        }

        public long LowestRange()
        {
            long result = long.MaxValue;

            foreach ((long start, long end, long range) in this.SeedRanges())
            {
                long skip = range;
                long current = start;

                while (current <= end)
                {
                    long value = current;

                    foreach (List<long[]> maps in this.Almanac)
                    {
                        foreach (long[] map in maps)
                        {
                            if (map[1] <= value && value < map[1] + map[2])
                            {
                                skip = Math.Min(skip, map[1] + map[2] - value);
                                skip = skip <= 0 ? 1 : skip;
                                value += map[0] - map[1];
                                break;
                            }
                        }
                    }

                    result = Math.Min(result, value);
                    current += skip;
                    skip = range - current + start;
                }
            }

            return result;
        }

        private IEnumerable<(long start, long end, long range)> SeedRanges()
            => this.Seeds.Skip(1).Zip(this.Seeds, (a, b) => (b, a + b, a)).Where((x, i) => i % 2 == 0);

        private long SeedToLocation(long seed)
        {
            MapType current = MapType.SeedToSoil;
            long value = seed;

            while (true)
            {
                long[] map = this.Almanac[(int)current].FirstOrDefault(x => value >= x[1] && value <= x[1] + (x[2] - 1)) ?? Array.Empty<long>();

                if (map.Any())
                {
                    value = map[0] + (value - map[1]);
                }

                current += 1;

                if ((int)current == 7)
                {
                    return value;
                }
            }
        }

        private List<List<long[]>> Parse(List<string> input)
        {
            List<List<long[]>> result = new();

            string[] tokens = input[0].Split("seeds: ");

            this.Seeds = tokens[1].Split(" ").Select(x => long.Parse(x)).ToList();

            input.RemoveRange(0, 2);

            MapType current = 0;
            result.Add(new());

            foreach (string value in input)
            {
                if (string.IsNullOrEmpty(value))
                {
                    current += 1;
                    result.Add(new());
                    continue;
                }

                if (char.IsDigit(value[0]))
                {
                    result[(int)current].Add(value.Split(" ").Select(x => long.Parse(x)).ToArray());
                }
            }

            return result;
        }
    }
}
