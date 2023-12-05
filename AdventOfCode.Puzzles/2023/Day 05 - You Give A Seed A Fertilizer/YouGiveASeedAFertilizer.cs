namespace AdventOfCode.Puzzles._2023.Day_05___You_Give_A_Seed_A_Fertilizer
{
    public class YouGiveASeedAFertilizer
    {
        public YouGiveASeedAFertilizer(List<string> input)
        {
            this.Seeds = new();
            this.Map = this.Parse(input);
        }

        private List<long> Seeds { get; set; }

        private Dictionary<MapType, List<List<long>>> Map { get; }

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

            for(long location = 0; location <= long.MaxValue; location++)
            {
                long seed = this.LocationToSeed(location);

                if (seed != long.MaxValue)
                {
                    for(int i = 0; i < this.Seeds.Count; i += 2)
                    {
                        if (seed >= this.Seeds[i] && seed <= this.Seeds[i] + this.Seeds[i + 1])
                        {
                            if (location < result)
                            {
                                return location;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private long SeedToLocation(long seed)
        {
            MapType current = MapType.SeedToSoil;
            long value = seed;

            while (true)
            {
                List<long> map = this.Map[current].FirstOrDefault(x => value >= x[1] && value <= x[1] + (x[2] - 1)) ?? new();

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

        private long LocationToSeed(long value)
        {
            MapType current = MapType.HumidityToLocation;

            while (true)
            {
                List<long> map = this.Map[current].FirstOrDefault(x => value >= x[0] && value <= x[0] + (x[2] - 1)) ?? new();

                if (map.Any())
                {
                    value = map[1] + (value - map[0]);
                }

                current -= 1;

                if ((int)current < 0)
                {
                    return value;
                }
            }
        }

        private Dictionary<MapType, List<List<long>>> Parse(List<string> input)
        {
            Dictionary<MapType, List<List<long>>> result = new();

            string[] tokens = input[0].Split("seeds: ");

            this.Seeds = tokens[1].Split(" ").Select(x => long.Parse(x)).ToList();

            input.RemoveRange(0, 2);

            MapType current = 0;
            this.Map.Add(current, new());

            foreach (string value in input)
            {
                if (string.IsNullOrEmpty(value))
                {
                    current += 1;
                    this.Map.Add(current, new());
                    continue;
                }

                if (char.IsDigit(value[0]))
                {
                    this.Map[current].Add(value.Split(" ").Select(x => long.Parse(x)).ToList());
                }
            }

            return result;
        }
    }
}
