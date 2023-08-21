namespace AdventOfCode.Puzzles._2021.Day_14___Extended_Polymerization
{
    public class ExtendedPolymerization
    {
        public ExtendedPolymerization(string[] input)
        {
            this.Template = string.Empty;
            this.Rules = new();

            bool rules = false;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    rules = true;
                    continue;
                }

                if (!rules)
                {
                    this.Template = line;

                    continue;
                }

                string[] rule = line.Split(" -> ");

                this.Rules.Add(rule[0], new());
                this.Rules[rule[0]].Add($"{rule[0][0]}{rule[1]}");
                this.Rules[rule[0]].Add($"{rule[1]}{rule[0][1]}");
            }
        }

        private string Template { get; }

        private Dictionary<string, List<string>> Rules { get; }

        public long Process(int steps)
        {
            Dictionary<string, long> counts = this.CountPairs();

            for (int step = 1; step <= steps; step++)
            {
                counts = this.CountStep(ref counts);
            }

            return SumOfMinAndMax(ref counts);
        }

        private static long SumOfMinAndMax(ref Dictionary<string, long> counts)
        {
            Dictionary<char, long> totals = new();

            foreach (KeyValuePair<string, long> kvp in counts)
            {
                if (!totals.ContainsKey(kvp.Key[1]))
                {
                    totals.Add(kvp.Key[1], counts[kvp.Key]);
                    continue;
                }

                totals[kvp.Key[1]] += counts[kvp.Key];
            }

            return totals.Max(x => x.Value) - totals.Min(x => x.Value);
        }

        private Dictionary<string, long> CountPairs()
        {
            Dictionary<string, long> counts = new();

            for (int i = 0; i < this.Template.Length - 1; i++)
            {
                string pair = $"{this.Template[i]}{this.Template[i + 1]}";

                if (!counts.ContainsKey(pair))
                {
                    counts.Add(pair, 1);
                    continue;
                }

                counts[pair] += 1;
            }

            return counts;
        }

        private Dictionary<string, long> CountStep(ref Dictionary<string, long> counts)
        {
            Dictionary<string, long> stepCounts = new();

            foreach (KeyValuePair<string, long> countsKvp in counts)
            {
                foreach (string nextPair in this.Rules[countsKvp.Key])
                {
                    if (!stepCounts.ContainsKey(nextPair))
                    {
                        stepCounts.Add(nextPair, counts[countsKvp.Key]);

                        continue;
                    }

                    stepCounts[nextPair] += counts[countsKvp.Key];
                }
            }

            return stepCounts;
        }
    }
}
