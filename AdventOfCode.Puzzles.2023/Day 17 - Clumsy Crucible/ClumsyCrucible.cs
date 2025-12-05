namespace AdventOfCode.Puzzles._2023.Day_17___Clumsy_Crucible
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ClumsyCrucible
    {
        public ClumsyCrucible(string[] input)
        {
            this.Map = new(input, (c) => int.Parse($"{c}"));
        }

        private VectorArray<int, int> Map { get; }

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
    }
}
