namespace AdventOfCode.Puzzles._2016.Day_20___Firewall_Rules
{
    using AdventOfCode.Core.Extensions;

    public class FirewallRules
    {
        public FirewallRules(string[] input)
        {
            this.Blacklist = ParseInput(input);
            this.Whitelist = new();

            this.GenerateWhitelist();
        }

        public List<(long Min, long Max)> Blacklist { get; }

        public List<long> Whitelist { get; }

        public long LowesetValuedIP() => this.Whitelist.First();

        public long CountOfAllowedIP() => this.Whitelist.Count;

        private static List<(long Min, long Max)> ParseInput(string[] input)
        {
            List<(long Min, long Max)> result = new();

            foreach (string line in input)
            {
                long[] numbers = line.Split("-").ToLong();

                result.Add((numbers[0], numbers[1]));
            }

            return result;
        }

        private FirewallRules GenerateWhitelist()
        {
            long ip = 0;

            while (ip <= this.Blacklist.Max(x => x.Max))
            {
                bool found = false;

                foreach ((long min, long max) in this.Blacklist)
                {
                    if (ip > max || ip < min)
                    {
                        continue;
                    }

                    ip = max + 1;
                    found = true;
                }

                if (found)
                {
                    continue;
                }

                this.Whitelist.Add(ip);
                ip++;
            }

            return this;
        }
    }
}
