namespace AdventOfCode.Puzzles._2019.Day_14___Space_Stoichiometry
{
    using AdventOfCode.Core.Extensions;

    public class SpaceStoichiometry
    {
        public SpaceStoichiometry(string[] input)
        {
            this.Reaction = new() { { "ORE", new Reaction("ORE") } };

            foreach (string line in input)
            {
                string[] formula = line.Split("=>");

                string[] item = formula[1].Trim().Split(" ");

                if (!this.Reaction.ContainsKey(item[1]))
                {
                    this.Reaction.Add(item[1], new(item[1], item[0].ToLong()));
                }

                Reaction result = this.Reaction[item[1]];
                result.Output = item[0].ToLong();

                foreach (string s in formula[0].Split(","))
                {
                    item = s.Trim().Split(" ");
                    result.AddSource(item[1], item[0].ToLong());

                    if (!this.Reaction.ContainsKey(item[1]))
                    {
                        this.Reaction.Add(item[1], new(item[1], 0));
                    }

                    this.Reaction[item[1]].AddProduct(result.Name, result.Output);
                }
            }
        }

        public Dictionary<string, Reaction> Reaction { get; }

        public long GetRequiredOre(long fuelTarget = 1)
        {
            IEnumerable<string> ordered = new Topological(this.Reaction).GetOrdered();
            Dictionary<string, long> quantity = new(ordered.Count()) { ["FUEL"] = fuelTarget };

            foreach (string item in ordered)
            {
                long output = this.Reaction[item].Output;
                long needed = quantity[item];
                long toMake = (long)Math.Ceiling((decimal)needed / output);

                foreach (var dependency in this.Reaction[item].GetDependencies())
                {
                    if (quantity.ContainsKey(dependency.Key))
                    {
                        quantity[dependency.Key] += dependency.Value * toMake;
                    }
                    else
                    {
                        quantity.Add(dependency.Key, dependency.Value * toMake);
                    }
                }
            }

            return quantity["ORE"];
        }

        public long GetTotalOre()
        {
            long requiredOre = this.GetRequiredOre();
            long target = 1000000000000;
            long lower = (target / requiredOre) - 1_000;
            long higher = (target / requiredOre) + 1_000_000_000;

            while (lower < higher)
            {
                long mid = (lower + higher) / 2;
                long guess = this.GetRequiredOre(mid);

                if (guess > target)
                {
                    higher = mid;
                }
                else if (guess < target)
                {
                    if (mid == lower)
                    {
                        break;
                    }

                    lower = mid;
                }
                else
                {
                    lower = mid;
                    break;
                }
            }

            return lower;
        }
    }
}
