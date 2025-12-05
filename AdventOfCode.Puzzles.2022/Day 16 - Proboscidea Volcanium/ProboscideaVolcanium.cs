namespace AdventOfCode.Puzzles._2022.Day_16___Proboscidea_Volcanium
{
    using AdventOfCode.Core.Extensions;

    public class ProboscideaVolcanium
    {
        private const string Start = "AA";

        public ProboscideaVolcanium(string[] input)
        {
            this.Distance = new int[0, 0];
            this.OpenValves = new();
            this.Masks = Array.Empty<int>();
            this.Valves = ParseValves(input);
            this.GeneratePaths();
        }

        private Dictionary<string, Valve> Valves { get; set; }

        private int[,] Distance { get; set; }

        private List<string> OpenValves { get; set; }

        private int[] Masks { get; set; }

        public int Single()
        {
            Dictionary<int, int> states = new();

            this.Cycle(0, 30, 0, 0, states);

            return states.Values.Max();
        }

        public int Pair()
        {
            Dictionary<int, int> states = new();

            this.Cycle(0, 26, 0, 0, states);

            int result = 0;

            foreach (var pairA in states)
            {
                foreach (var pairB in states)
                {
                    if ((pairA.Key & pairB.Key) != 0)
                    {
                        continue;
                    }

                    result = Math.Max(result, pairA.Value + pairB.Value);
                }
            }

            return result;
        }

        private static Dictionary<string, Valve> ParseValves(string[] input)
        {
            Dictionary<string, Valve> result = new();

            foreach (string line in input)
            {
                string[] tokens = line.Split("; ");
                string[] valveTokens = tokens[0].Replace("Valve ").Split(" has flow rate=");
                result.Add(valveTokens[0], new(valveTokens[0], valveTokens[1].ToInt()));
            }

            for (int i = 0; i < input.Length; i++)
            {
                Valve current = result.ElementAt(i).Value;

                string[] tokens = input[i].Split("; ");

                if (tokens[1].IndexOf("tunnels lead to valves") != -1)
                {
                    current.Tunnels.AddRange(tokens[1].Replace("tunnels lead to valves").Trim().Split(", ").Select(x => result[x].Name));
                }
                else
                {
                    current.Tunnels.Add(tokens[1].Replace("tunnel leads to valve").Trim());
                }
            }

            return result;
        }

        private void GeneratePaths()
        {
            List<string> valves = this.Valves.Values.OrderBy(a => a.Name).Select(a => a.Name).ToList();
            this.Distance = this.GenerateDistances(valves);
            this.OpenValves = valves.Where(a => a == Start || this.Valves[a].FlowRate > 0).ToList();
            this.Masks = this.GenerateMask();
        }

        private int[,] GenerateDistances(List<string> valves)
        {
            int[,] result = new int[this.Valves.Count, this.Valves.Count];

            for (int i = 0; i < valves.Count; i++)
            {
                for (int j = i; j < valves.Count; j++)
                {
                    if (i == j)
                    {
                        result[i, j] = 0;
                    }
                    else if (this.Valves[valves[i]].Tunnels.Contains(valves[j]))
                    {
                        result[i, j] = result[j, i] = 1;
                    }
                    else
                    {
                        result[i, j] = result[j, i] = 1000;
                    }
                }
            }

            for (int k = 0; k < valves.Count; k++)
            {
                for (int i = 0; i < valves.Count; i++)
                {
                    for (int j = i + 1; j < valves.Count; j++)
                    {
                        if (result[i, k] + result[k, j] < result[i, j])
                        {
                            result[i, j] = result[j, i] = result[i, k] + result[k, j];
                        }
                    }
                }
            }

            List<int> index = this.GenerateIndex(valves);

            foreach (var i in index)
            {
                result = result.Trim(i, i);
            }

            return result;
        }

        private List<int> GenerateIndex(List<string> valves)
        {
            List<int> result = new();

            for (int i = 0; i < valves.Count; i++)
            {
                if (this.Valves[valves[i]].FlowRate == 0 && valves[i] != Start)
                {
                    result.Add(i);
                }
            }

            result.Reverse();

            return result;
        }

        private int[] GenerateMask()
        {
            int[] result = new int[this.Distance.GetLength(0)];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = 1 << i;
            }

            return result;
        }

        private void Cycle(int node, int minute, int state, int flow, Dictionary<int, int> states)
        {
            states[state] = Math.Max(states.GetValueOrDefault(state, 0), flow);

            for (int i = 0; i < this.OpenValves.Count; i++)
            {
                int next = minute - this.Distance[node, i] - 1;

                if ((this.Masks[i] & state) != 0 || next <= 0)
                {
                    continue;
                }

                this.Cycle(i, next, state | this.Masks[i], flow + (next * this.Valves[this.OpenValves[i]].FlowRate), states);
            }
        }
    }
}
