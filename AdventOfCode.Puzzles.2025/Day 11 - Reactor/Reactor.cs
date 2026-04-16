namespace AdventOfCode.Puzzles._2025.Day_11___Reactor
{
    using System;
    using System.Collections.Generic;

    public class Reactor
    {
        public string StartDevice = "you";

        public string EndDevice = "out";

        public string ServerDevice = "svr";

        public string DigitalToAnalogDevice = "dac";

        public string FourierTransformDevice = "fft";

        private Dictionary<string, HashSet<string>> Adjacent { get; }

        private Dictionary<string, HashSet<string>> ReverseAdjacent { get; }

        public Reactor(string[] input)
        {
            this.Adjacent = [];
            this.ReverseAdjacent = [];
            this.Parse(input);
        }

        private void Parse(string[] input)
        {
            foreach (string line in input)
            {
                string[] tokens = line.Split(':');
                string device = tokens[0].Trim();
                string[] outputs = tokens.Length > 1
                    ? tokens[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    : [];

                if (!this.Adjacent.ContainsKey(device))
                {
                    this.Adjacent[device] = [];
                }

                if (!this.ReverseAdjacent.ContainsKey(device))
                {
                    this.ReverseAdjacent[device] = [];
                }

                foreach (string output in outputs)
                {
                    this.Adjacent[device].Add(output);

                    if (!this.Adjacent.ContainsKey(output))
                    {
                        this.Adjacent[output] = [];
                    }

                    if (!this.ReverseAdjacent.ContainsKey(output))
                    {
                        this.ReverseAdjacent[output] = [];
                    }

                    this.ReverseAdjacent[output].Add(device);
                }
            }
        }

        public long CountAllPaths() => this.PathCountPruned(this.StartDevice, this.EndDevice);

        public long CountServerPaths()
        {
            List<string> sorted = [.. TopologicalSort(this.ServerDevice, this.Adjacent)];

            Dictionary<string, int> positions = [];
            
            for (int i = 0; i < sorted.Count; i++)
            {
                positions[sorted[i]] = i;
            }

            string deviceA = this.FourierTransformDevice;
            string deviceB = this.DigitalToAnalogDevice;

            if (positions[this.FourierTransformDevice] > positions[this.DigitalToAnalogDevice])
            {
                deviceA = this.DigitalToAnalogDevice;
                deviceB = this.FourierTransformDevice;
            }

            long a = this.PathCountPruned(this.ServerDevice, deviceA);
            long b = this.PathCountPruned(deviceA, deviceB);
            long c = this.PathCountPruned(deviceB, this.EndDevice);

            return a * b * c;
        }

        private long PathCountPruned(string start, string dest)
        {
            HashSet<string> valid = ReachableFrom(start, this.Adjacent);
            valid.IntersectWith(ReachableFrom(dest, this.ReverseAdjacent));

            Dictionary<string, List<string>> pruned = [];
            Dictionary<string, int> indegree = [];

            foreach (string node in valid)
            {
                pruned[node] = [];
                indegree[node] = 0;
            }

            foreach (string node in valid)
            {
                if (!this.Adjacent.TryGetValue(node, out HashSet<string> outputs))
                {
                    continue;
                }

                foreach (string output in outputs)
                {
                    if (!valid.Contains(output))
                    {
                        continue;
                    }

                    pruned[node].Add(output);
                    indegree[output]++;
                }
            }

            Queue<string> queue = new();

            foreach ((string node, int degree) in indegree)
            {
                if (degree == 0)
                {
                    queue.Enqueue(node);
                }
            }

            List<string> topo = [];

            while (queue.Count > 0)
            {
                string node = queue.Dequeue();
                topo.Add(node);

                foreach (string next in pruned[node])
                {
                    indegree[next]--;
                    if (indegree[next] == 0)
                    {
                        queue.Enqueue(next);
                    }
                }
            }

            Dictionary<string, long> ways = [];

            foreach (string node in valid)
            {
                ways[node] = 0L;
            }

            ways[start] = 1L;

            foreach (string node in topo)
            {
                long count = ways[node];
                if (count == 0)
                {
                    continue;
                }

                foreach (string next in pruned[node])
                {
                    ways[next] += count;
                }
            }

            return ways[dest];
        }

        private static HashSet<string> ReachableFrom(
            string start,
            Dictionary<string, HashSet<string>> adjacent)
        {
            HashSet<string> visited = [];
            Stack<string> stack = new();
            stack.Push(start);

            while (stack.Count > 0)
            {
                string device = stack.Pop();

                if (!visited.Add(device))
                {
                    continue;
                }

                if (!adjacent.TryGetValue(device, out HashSet<string> outputs))
                {
                    continue;
                }

                foreach (string output in outputs)
                {
                    if (!visited.Contains(output))
                    {
                        stack.Push(output);
                    }
                }
            }

            return visited;
        }

        private static List<string> TopologicalSort(
            string start,
            Dictionary<string, HashSet<string>> adjacent)
        {
            HashSet<string> visited = [];
            List<string> result = [];

            void Dfs(string device)
            {
                if (!visited.Add(device))
                {
                    return;
                }

                if (adjacent.TryGetValue(device, out HashSet<string> outputs))
                {
                    foreach (string output in outputs)
                    {
                        Dfs(output);
                    }
                }

                result.Add(device);
            }

            Dfs(start);
            result.Reverse();

            return result;
        }
    }
}