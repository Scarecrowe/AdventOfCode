namespace AdventOfCode.Puzzles._2015.Day_09___All_in_a_Single_Night
{
    using AdventOfCode.Core.Extensions;

    public class AllInASingleNight
    {
        public AllInASingleNight(string[] input)
        {
            this.Locations = new();

            foreach (string line in input)
            {
                Journey journey = Parse(line);

                if (!this.Locations.ContainsKey(journey.Source))
                {
                    this.Locations.Add(journey.Source, new());
                }

                this.Locations[journey.Source].Add(journey.Destination, journey.Distance);

                if (!this.Locations.ContainsKey(journey.Destination))
                {
                    this.Locations.Add(journey.Destination, new());
                }

                this.Locations[journey.Destination].Add(journey.Source, journey.Distance);
            }
        }

        private Dictionary<string, Dictionary<string, int>> Locations { get; set; }

        public int Shortest() => this.Sort().Values.Min();

        public int Longest() => this.Sort().Values.Max();

        private static Journey Parse(string line)
        {
            string[] tokens = line.Split(" ");

            return new(tokens[0], tokens[2], tokens[4]);
        }

        private Dictionary<string, int> Sort()
        {
            Dictionary<string, int> routes = new();

            List<string> locations = this.Locations.Select(x => x.Key).ToList();
            List<List<string>> permutations = locations.Permutations(locations.Count);

            foreach (List<string> route in permutations)
            {
                int distance = 0;

                for (int i = 0; i < route.Count - 1; i++)
                {
                    distance += this.Locations[route[i]][route[i + 1]];
                }

                routes.Add(route.Join(" -> "), distance);
            }

            return routes;
        }
    }
}
