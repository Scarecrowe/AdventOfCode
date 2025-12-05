namespace AdventOfCode.Puzzles._2018.Day_23___Experimental_Emergency_Teleportation
{
    public class ExperimentalEmergencyTeleportation
    {
        public ExperimentalEmergencyTeleportation(string[] input) => this.Nanobots = Parse(input);

        public List<Nanobot> Nanobots { get; private set; }

        public int LargestRadius()
        {
            foreach (Nanobot nanobotA in this.Nanobots)
            {
                foreach (Nanobot nanobotB in this.Nanobots)
                {
                    if (nanobotA == nanobotB)
                    {
                        continue;
                    }

                    if (nanobotA.Point.Distance(nanobotB.Point) <= nanobotA.Radius)
                    {
                        nanobotA.Count++;
                    }
                }
            }

            int max = this.Nanobots.Max(d => d.Radius);

            return this.Nanobots.First(c => c.Radius == max).Count + 1;
        }

        public int DistanceOfLargestPoint()
        {
            Dictionary<int, int> map = new();
            int current = 0;
            int result = 0;
            int min = 0;
            int max = 0;

            foreach (Nanobot nanobot in this.Nanobots)
            {
                min = nanobot.Point.X + nanobot.Point.Y + nanobot.Point.Z - nanobot.Radius;
                max = nanobot.Point.X + nanobot.Point.Y + nanobot.Point.Z + nanobot.Radius + 1;

                if (!map.ContainsKey(min))
                {
                    map.Add(min, 0);
                }

                map[min]++;

                if (!map.ContainsKey(max))
                {
                    map.Add(max, 0);
                }

                map[max]--;
            }

            max = 0;

            foreach (var item in map)
            {
                current += item.Value;
                if (current > max)
                {
                    max = current;
                    result = item.Key;
                }
            }

            return map.OrderByDescending(c => Math.Abs(c.Value)).First().Key;
        }

        private static List<Nanobot> Parse(string[] input) => input.Select(x => new Nanobot(x)).ToList();
    }
}
