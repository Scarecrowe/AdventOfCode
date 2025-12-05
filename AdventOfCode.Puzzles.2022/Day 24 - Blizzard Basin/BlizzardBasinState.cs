namespace AdventOfCode.Puzzles._2022.Day_24___Blizzard_Basin
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class BlizzardBasinState
    {
        public BlizzardBasinState(int minutes, Vector<int> point)
        {
            this.Minutes = minutes;
            this.Point = point;
            this.Queue = new();
            this.Visited = new();
        }

        public int Minutes { get; private set; }

        public Vector<int> Point { get; private set; }

        private PriorityQueue<BlizzardBasinState, int> Queue { get; }

        private HashSet<(Vector<int>, int)> Visited { get; }

        public BlizzardBasinState Clone() => new(this.Minutes, new(this.Point));

        public BlizzardBasinState SetPoint(Vector<int> point)
        {
            this.Point = point;

            return this;
        }

        public BlizzardBasinState Tick()
        {
            this.Minutes++;

            return this;
        }

        public int Distance(Vector<int> finish) => this.Minutes + (int)finish.Distance(this.Point);

        public IEnumerable<BlizzardBasinState> Adjacent(BlizzardBasinMaps maps)
        {
            foreach (Vector<int> point in this.Directions())
            {
                if (maps.GetMap(this.Minutes + 1, point) == BlizzardBasinType.Empty)
                {
                    yield return this.Clone().Tick().SetPoint(point);
                }
            }
        }

        public BlizzardBasinState Move(Vector<int> finish, BlizzardBasinMaps maps)
        {
            this.Visited.Clear();
            this.Queue.Clear();
            this.Enqueue(this, finish);

            while (this.Queue.Any())
            {
                BlizzardBasinState current = this.Queue.Dequeue();

                if (current.Point == finish)
                {
                    return current;
                }

                foreach (BlizzardBasinState adjacent in current.Adjacent(maps))
                {
                    if (this.Visited.Contains((adjacent.Point, adjacent.Minutes)))
                    {
                        continue;
                    }

                    this.Visited.Add((adjacent.Point, adjacent.Minutes));
                    this.Enqueue(adjacent, finish);
                }
            }

            throw new InvalidOperationException();
        }

        private List<Vector<int>> Directions()
        {
            return new()
            {
                this.Point,
                new(this.Point.X, this.Point.Y - 1),
                new(this.Point.X, this.Point.Y + 1),
                new(this.Point.X - 1, this.Point.Y),
                new(this.Point.X + 1, this.Point.Y)
            };
        }

        private void Enqueue(BlizzardBasinState state, Vector<int> finish) => this.Queue.Enqueue(state, state.Distance(finish));
    }
}
