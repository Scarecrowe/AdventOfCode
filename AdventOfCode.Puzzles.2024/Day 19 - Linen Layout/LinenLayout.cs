using AdventOfCode.Core;

namespace AdventOfCode.Puzzles._2024.Day_19___Linen_Layout
{
    public class LinenLayout
    {
        public List<string> Patterns { get; private set; }

        public HashSet<string> Designs { get; private set; }

        Dictionary<string, bool> memo = new();

        DefaultDictionary<string, long> counts = new();
        long p2res = 0;

        public LinenLayout(string[] input)
        {
            this.Patterns = [];
            this.Designs = [];
            this.Parse(input);
        }

        public int Possible()
        {
            int count = 0;

            foreach (var design in this.Designs)
            {
                (bool succ, long resCount) = DFS(design);
                if (succ)
                {
                    count++;
                    p2res += resCount;
                }
            }

            return count;
        }

        public long Arrangements()
        {
            long result = 0;

            foreach (var design in this.Designs)
            {
                (bool succ, long resCount) = DFS(design);
                if (succ)
                {
                    result += resCount;
                }
            }

            return result;
        }

        private void Parse(string[] input)
        {
            bool designs = false;
            string patterns = string.Empty;

            foreach (string line in input)
            {
                if(string.IsNullOrEmpty(line))
                {
                    designs = true;
                    this.Patterns = [.. patterns.Split(", ")];
                    continue;
                }

                if (!designs)
                {
                    patterns += line;
                    continue;
                }

                this.Designs.Add(line);
            }
        }

        private (bool match, long ways) DFS(string design)
        {
            if (memo.TryGetValue(design, out bool value))
            {
                return (value, counts[design]);
            }

            if (this.Patterns.Contains(design))
            {
                memo[design] = true;
                counts[design]++;
            }

            foreach (var t in this.Patterns.Where(t => t.Length < design.Length && design[..t.Length] == t))
            {
                string sub = design[t.Length..];
                var (match, ways) = DFS(sub);
                if (match)
                {
                    memo[design] = true;
                    counts[design] += ways;
                }
            }

            memo[design] = counts[design] > 0;

            return (memo[design], counts[design]);
        }
    }
}
