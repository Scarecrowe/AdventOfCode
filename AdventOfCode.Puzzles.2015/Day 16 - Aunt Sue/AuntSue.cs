namespace AdventOfCode.Puzzles._2015.Day_16___Aunt_Sue
{
    using AdventOfCode.Core.Extensions;

    public class AuntSue
    {
        public AuntSue(string[] input) => this.AuntSues = Parse(input);

        private static Dictionary<string, int> DetectedCompounds { get; } = new()
        {
            { "children", 3 },
            { "cats", 7 },
            { "samoyeds", 2 },
            { "pomeranians", 3 },
            { "akitas", 0 },
            { "vizslas", 0 },
            { "goldfish", 5 },
            { "trees", 3 },
            { "cars", 2 },
            { "perfumes", 1 }
        };

        private Dictionary<int, Dictionary<string, int>> AuntSues { get; set; }

        public int FindSue()
        {
            foreach (KeyValuePair<int, Dictionary<string, int>> sue in this.AuntSues)
            {
                bool match = true;

                foreach (KeyValuePair<string, int> compound in DetectedCompounds)
                {
                    if (!sue.Value.ContainsKey(compound.Key))
                    {
                        continue;
                    }

                    switch (compound.Key)
                    {
                        case "cats":
                        case "trees":
                            if (sue.Value[compound.Key] <= compound.Value)
                            {
                                match = false;
                            }

                            break;
                        case "pomeranians":
                        case "goldfish":
                            if (sue.Value[compound.Key] >= compound.Value)
                            {
                                match = false;
                            }

                            break;
                        default:
                            if (sue.Value[compound.Key] != compound.Value)
                            {
                                match = false;
                            }

                            break;
                    }

                    if (!match)
                    {
                        break;
                    }
                }

                if (match)
                {
                    return sue.Key;
                }
            }

            return 0;
        }

        public int FindExactSue()
        {
            foreach (KeyValuePair<int, Dictionary<string, int>> sue in this.AuntSues)
            {
                bool match = true;

                foreach (KeyValuePair<string, int> compound in DetectedCompounds)
                {
                    if (sue.Value.ContainsKey(compound.Key)
                        && sue.Value[compound.Key] != compound.Value)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    return sue.Key;
                }
            }

            return 0;
        }

        private static Dictionary<int, Dictionary<string, int>> Parse(string[] input)
        {
            Dictionary<int, Dictionary<string, int>> result = new();

            int index = 0;

            input.ForEach(line => result.Add(++index, ParseSue(line)));

            return result;
        }

        private static Dictionary<string, int> ParseSue(string input)
        {
            Dictionary<string, int> sue = new();

            string[] tokens = input[(input.IndexOf(":") + 1) ..].Split(",");

            foreach (string token in tokens)
            {
                string[] values = token.Trim().Split(": ");

                sue.Add(values[0], values[1].ToInt());
            }

            return sue;
        }
    }
}
