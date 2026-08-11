namespace AdventOfCode.Puzzles._2023.Day_17___Clumsy_Crucible
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Linq;
    using System.Text;

    public class ClumsyCrucible
    {
        public ClumsyCrucible(string[] input)
        {
            this.Map = new(input, (c) => int.Parse($"{c}"));
        }

        public ClumsyCrucible(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, int> Map { get; }

        private sealed record PathNode(State State, int HeatLoss);

        public int CrucibleHeatLoss()
            => this.Move((state, cell) =>
            {
                return cell.Direction == state.Direction && state.Distance == 3;
            });

        public int UltraCrucibleHeatLoss()
            => this.Move((state, cell) =>
            {
                return (cell.Direction != state.Direction && state.Distance < 4)
                    || (cell.Direction == state.Direction && state.Distance == 10);
            });

        public ClumsyCrucible RenderSilver(int renderEvery = 8)
        {
            return this.RenderPath(
                title: "CLUMSY CRUCIBLE",
                completeTitle: "CRUCIBLE DELIVERED",
                renderEvery,
                comparer: (state, cell) => cell.Direction == state.Direction && state.Distance == 3,
                canStop: _ => true);
        }

        public ClumsyCrucible RenderGold(int renderEvery = 8)
        {
            return this.RenderPath(
                title: "ULTRA CLUMSY CRUCIBLE",
                completeTitle: "ULTRA CRUCIBLE DELIVERED",
                renderEvery,
                comparer: (state, cell) =>
                    (cell.Direction != state.Direction && state.Distance < 4)
                    || (cell.Direction == state.Direction && state.Distance == 10),
                canStop: state => state.Distance >= 4);
        }

        private int Move(Func<State, VectorCell<int, int>, bool> comparer)
        {
            PriorityQueue<State, int> queue = new();
            Vector<int> finish = new(this.Map.Width - 1, this.Map.Height - 1);
            Dictionary<State, int> states = new()
            {
                { new State(new(0, 0), Cardinal.East, 0), 0 },
                { new State(new(0, 0), Cardinal.South, 0), 0 }
            };

            queue.Enqueue(states.ElementAt(0).Key, 0);
            queue.Enqueue(states.ElementAt(1).Key, 0);

            while (queue.Any())
            {
                queue.TryPeek(out State? state, out int priority);

                if (state?.Point == finish)
                {
                    return priority;
                }

                state = queue.Dequeue();

                Cardinal turn = CardinalHelper.Flip(state.Direction);

                foreach (VectorCell<int, int> cell in this.Map
                    .AdjacentCardinal(state.Point)
                    .Where(x => x.Direction != turn && !comparer(state, x))
                    .Select(x => new VectorCell<int, int>(state.Point.Clone().Transform(x.Direction), x.Direction)))
                {
                    int value = states[state] + this.Map[cell.Point];
                    int current = states.ContainsKey(state.Next(cell)) ? states[state.Next(cell)] : int.MaxValue;

                    if (value < current)
                    {
                        states[state.Next(cell)] = value;
                        queue.Enqueue(state.Next(cell), value + cell.Point.Distance(finish));
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private ClumsyCrucible RenderPath(
            string title,
            string completeTitle,
            int renderEvery,
            Func<State, VectorCell<int, int>, bool> comparer,
            Func<State, bool> canStop)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<PathNode> path = this.FindPath(comparer, canStop);

            if (path.Count == 0)
            {
                return this;
            }

            List<string[]> frames = [];
            Dictionary<Vector<int>, char> trail = [];

            for (int i = 0; i < path.Count; i++)
            {
                PathNode node = path[i];

                if (i > 0)
                {
                    PathNode previous = path[i - 1];
                    trail[previous.State.Point] = ToArrow(node.State.Direction);
                }

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        node.State.Point,
                        trail,
                        $"{title} // HEAT LOSS {node.HeatLoss:00000} // STEP {i:00000}"));
                }
            }

            PathNode last = path.Last();

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    last.State.Point,
                    trail,
                    $"{completeTitle} // HEAT LOSS {last.HeatLoss:00000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<PathNode> FindPath(
            Func<State, VectorCell<int, int>, bool> comparer,
            Func<State, bool> canStop)
        {
            PriorityQueue<State, int> queue = new();
            Vector<int> finish = new(this.Map.Width - 1, this.Map.Height - 1);

            Dictionary<State, int> heatLoss = new()
            {
                { new State(new(0, 0), Cardinal.East, 0), 0 },
                { new State(new(0, 0), Cardinal.South, 0), 0 }
            };

            Dictionary<State, State> cameFrom = [];
            HashSet<State> visited = [];

            queue.Enqueue(heatLoss.ElementAt(0).Key, 0);
            queue.Enqueue(heatLoss.ElementAt(1).Key, 0);

            while (queue.Count > 0)
            {
                State state = queue.Dequeue();

                if (!visited.Add(state))
                {
                    continue;
                }

                if (state.Point == finish && canStop(state))
                {
                    return ReconstructPath(state, cameFrom, heatLoss);
                }

                Cardinal turn = CardinalHelper.Flip(state.Direction);

                foreach (VectorCell<int, int> cell in this.Map
                    .AdjacentCardinal(state.Point)
                    .Where(x => x.Direction != turn && !comparer(state, x))
                    .Select(x => new VectorCell<int, int>(state.Point.Clone().Transform(x.Direction), x.Direction)))
                {
                    State next = state.Next(cell);
                    int value = heatLoss[state] + this.Map[cell.Point];
                    int current = heatLoss.TryGetValue(next, out int existing) ? existing : int.MaxValue;

                    if (value < current)
                    {
                        heatLoss[next] = value;
                        cameFrom[next] = state;
                        queue.Enqueue(next, value + cell.Point.Distance(finish));
                    }
                }
            }

            return [];
        }

        private static List<PathNode> ReconstructPath(
            State target,
            Dictionary<State, State> cameFrom,
            Dictionary<State, int> heatLoss)
        {
            List<State> states = [target];
            State current = target;

            while (cameFrom.TryGetValue(current, out State? previous))
            {
                states.Add(previous);
                current = previous;
            }

            states.Reverse();

            return states
                .Select(state => new PathNode(state, heatLoss[state]))
                .ToList();
        }

        private string[] BuildFrame(
            Vector<int> current,
            Dictionary<Vector<int>, char> trail,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (trail.TryGetValue(point, out char direction))
                    {
                        sb.Append(direction);
                    }
                    else
                    {
                        sb.Append(this.Map[y, x]);
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(
                    new Frame(PadFrame(frame, width, height)));
            }
        }

        private static string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }

        private static char ToArrow(Cardinal direction)
            => direction switch
            {
                Cardinal.North => '^',
                Cardinal.East => '>',
                Cardinal.South => 'v',
                Cardinal.West => '<',
                _ => '?'
            };
    }
}
