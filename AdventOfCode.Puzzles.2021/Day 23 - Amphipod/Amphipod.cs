using AdventOfCode.Animation.Renderers;

namespace AdventOfCode.Puzzles._2021.Day_23___Amphipod
{
    public class Amphipod
    {
        public Amphipod(string[] input, bool advanced = false)
            => this.Map = new(input, advanced);

        public Amphipod(string[] input, IFrameRenderer renderer, bool advanced = false)
            : this(input, advanced)
        {
            this.Renderer = renderer;
        }

        private AmphipodMap Map { get; }

        private IFrameRenderer? Renderer { get; }

        public int Run()
        {
            PriorityQueue<AmphipodMap, int> queue = new();
            queue.Enqueue(this.Map, this.Map.TotalEnergy);

            HashSet<string> visited = [];

            while (queue.Count > 0)
            {
                AmphipodMap map = queue.Dequeue();
                string key = map.ToKey();

                if (!visited.Add(key))
                {
                    continue;
                }

                if (map.IsComplete())
                {
                    return map.TotalEnergy;
                }

                foreach (AmphipodMap next in map.NextMaps())
                {
                    queue.Enqueue(next, next.TotalEnergy);
                }
            }

            throw new InvalidOperationException();
        }

        public Amphipod RenderSilver()
            => this.RenderPath("AMPHIPOD", false);

        public Amphipod RenderGold()
            => this.RenderPath("AMPHIPOD UNFOLDED", true);

        private Amphipod RenderPath(string title, bool advanced)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<AmphipodMap> path = this.FindPath();

            for (int i = 0; i < path.Count; i++)
            {
                AmphipodMap? previous = i == 0 ? null : path[i - 1];

                this.Renderer.RenderFrame(
                    new Frame(path[i].ToFrame(
                        $"{title} // ENERGY {path[i].TotalEnergy:00000}",
                        previous)));
            }

            AmphipodMap last = path.Last();

            this.Renderer.RenderFrame(
                    new Frame(last.ToFrame(
                        $"COMPLETE // ENERGY {last.TotalEnergy:00000}")));

            return this;
        }

        private List<AmphipodMap> FindPath()
        {
            PriorityQueue<AmphipodMap, int> queue = new();
            Dictionary<string, int> best = [];
            Dictionary<string, string> cameFrom = [];
            Dictionary<string, AmphipodMap> states = [];

            string startKey = this.Map.ToKey();

            queue.Enqueue(this.Map, 0);
            best[startKey] = 0;
            states[startKey] = this.Map;

            while (queue.Count > 0)
            {
                AmphipodMap current = queue.Dequeue();
                string currentKey = current.ToKey();

                if (current.IsComplete())
                {
                    return Reconstruct(cameFrom, states, currentKey);
                }

                foreach (AmphipodMap next in current.NextMaps())
                {
                    string nextKey = next.ToKey();

                    if (best.TryGetValue(nextKey, out int existing)
                        && existing <= next.TotalEnergy)
                    {
                        continue;
                    }

                    best[nextKey] = next.TotalEnergy;
                    cameFrom[nextKey] = currentKey;
                    states[nextKey] = next;

                    queue.Enqueue(next, next.TotalEnergy);
                }
            }

            return [];
        }

        private static List<AmphipodMap> Reconstruct(
            Dictionary<string, string> cameFrom,
            Dictionary<string, AmphipodMap> states,
            string target)
        {
            List<AmphipodMap> path = [];
            string current = target;

            path.Add(states[current]);

            while (cameFrom.TryGetValue(current, out string? previous))
            {
                current = previous;
                path.Add(states[current]);
            }

            path.Reverse();

            return path;
        }
    }
}