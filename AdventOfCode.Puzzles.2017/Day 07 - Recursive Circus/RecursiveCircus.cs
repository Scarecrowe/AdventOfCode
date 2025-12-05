namespace AdventOfCode.Puzzles._2017.Day_07___Recursive_Circus
{
    using AdventOfCode.Core.Extensions;

    public class RecursiveCircus
    {
        public RecursiveCircus(string[] input)
        {
            this.Discs = new();

            foreach (string line in input)
            {
                Disc disc = Parse(line);

                this.Discs.Add(disc.Name, disc);
            }

            var discs = this.Discs;

            foreach (KeyValuePair<string, Disc> disc in this.Discs)
            {
                disc.Value.CalculateTotalWeight(ref discs);
            }
        }

        public Dictionary<string, Disc> Discs { get; }

        public static Disc Parse(string input)
        {
            string[] tokens = input.Split(" -> ");
            string[] disc = tokens[0].SplitSpace();

            Disc result = new(disc[0].Trim(), disc[1].Trim().Replace("(", string.Empty).Replace(")", string.Empty).ToInt());

            if (tokens.Length > 1)
            {
                result.Children.AddRange(tokens[1].Trim().Split(", ").ToList());
            }

            return result;
        }

        public string BottomProgram()
        {
            foreach (KeyValuePair<string, Disc> discA in this.Discs)
            {
                if (!discA.Value.Children.Any())
                {
                    continue;
                }

                bool found = true;

                foreach (KeyValuePair<string, Disc> discB in this.Discs)
                {
                    if (discA.Key == discB.Key || !discB.Value.Children.Any())
                    {
                        continue;
                    }

                    if (discB.Value.Children.Contains(discA.Key))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return discA.Key;
                }
            }

            throw new InvalidOperationException();
        }

        public int BalancedWeight()
        {
            foreach (KeyValuePair<string, Disc> disc in this.Discs)
            {
                if (!disc.Value.Children.Any())
                {
                    continue;
                }

                var group = disc.Value.Children.Select(x => this.Discs[x]).GroupBy(x => x.TotalWeight).ToList();

                if (group.Any(x => x.Count() == 1))
                {
                    var unbalanced = this.Discs.FirstOrDefault(x => x.Value.TotalWeight == group.First(x => x.Count() == 1).Key);

                    int diff = group.First(x => x.Count() == 1).Key - group.First(x => x.Count() > 1).Key;

                    return unbalanced.Value.Weight - diff;
                }
            }

            throw new InvalidOperationException();
        }
    }
 }
