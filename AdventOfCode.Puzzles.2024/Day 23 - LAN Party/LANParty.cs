namespace AdventOfCode.Puzzles._2024.Day_23___LAN_Party
{
    public class LANParty
    {
        public Dictionary<string, HashSet<string>> Connections { get; private set; }

        public LANParty(string[] input)
        {
            this.Connections = [];
            this.Parse(input);
        }

        private void Parse(string[] input)
        {
            foreach (string line in input)
            {
                string[] computers = line.Split("-");

                this.AddConnection(computers[0], computers[1]);
                this.AddConnection(computers[1], computers[0]);
            }
        }

        private void AddConnection(string computerA, string computerB)
        {
            if (!this.Connections.TryGetValue(computerA, out var value))
            {
                this.Connections[computerA] = [computerB];
                return;
            }
            
            value.Add(computerB);
        }

        private List<(string A, string B, string C)> KeyCombinations()
        {
            List<string> keys = [.. this.Connections.Keys];
            List<(string, string, string)> result = [];

            for (int i = 0; i < keys.Count - 2; i++)
            {
                for (int j = i + 1; j < keys.Count - 1; j++)
                {
                    for (int k = j + 1; k < keys.Count; k++)
                    {
                        if (!keys[i].StartsWith("t")
                            && !keys[j].StartsWith("t")
                            && !keys[k].StartsWith("t"))
                        {
                            continue;
                        }

                        result.Add((keys[i], keys[j], keys[k]));
                    }
                }
            }

            return result;
        }

        private List<string> LargestConnection(Dictionary<string, HashSet<string>> connections)
        {
            List<HashSet<string>> results = [];
            List<string> keys = [.. connections.Keys];

            void Search(HashSet<string> connection, HashSet<string> remainingKeys, HashSet<string> skipKeys)
            {
                if (remainingKeys.Count == 0
                    && skipKeys.Count == 0)
                {
                    if (!results.Any(x => x.SetEquals(connection)))
                    {
                        results.Add(new HashSet<string>(connection));
                    }

                    return;
                }

                foreach (string key in remainingKeys.ToList())
                {
                    Search(
                        new(connection) { key },
                        new(remainingKeys.Intersect(connections[key])),
                        new(skipKeys.Intersect(connections[key])));

                    remainingKeys.Remove(key);
                    skipKeys.Add(key);
                }
            }

            Search([], new HashSet<string>(keys), []);

            int maxSize = results.Max(x => x.Count);

            List<string> sorted = new(results.First(x => x.Count == maxSize));
            sorted.Sort();

            return sorted;
        }

        public int Interconnections()
        {
            int result = 0;

            foreach (var (a, b, c) in this.KeyCombinations())
            {
                if (this.Connections[a].Contains(b)
                    && this.Connections[a].Contains(c)
                    && this.Connections[b].Contains(a)
                    && this.Connections[b].Contains(c)
                    && this.Connections[c].Contains(a)
                    && this.Connections[c].Contains(b))
                {
                    result++;
                }
            }

            return result;
        }

        public string Password() => string.Join(",", this.LargestConnection(this.Connections));
    }
}
