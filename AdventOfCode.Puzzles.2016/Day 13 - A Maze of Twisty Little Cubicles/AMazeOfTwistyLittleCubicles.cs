namespace AdventOfCode.Puzzles._2016.Day_13___A_Maze_of_Twisty_Little_Cubicles
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class AMazeOfTwistyLittleCubicles
    {
        public AMazeOfTwistyLittleCubicles(string[] input, int targetX, int targetY)
        {
            int favorite = input[0].ToInt();
            this.Target = new(targetX, targetY);
            this.Paths = new();
            this.Visited = new();
            this.Map = CreateMap(favorite, this.Target + 10);
        }

        public VectorDictionary<int, int> Map { get; }

        public Vector<int> Target { get; }

        private List<long> Paths { get; set; }

        private HashSet<Vector<int>> Visited { get; set; }

        public long ShortestPath() => this.Paths.Min();

        public long UniqueLocations() => this.Visited.Count;

        public AMazeOfTwistyLittleCubicles FindAllPaths()
        {
            this.Paths = new();
            this.Visited = new();
            this.FindPath(new(1, 1), 0, new() { new(1, 1) });

            return this;
        }

        private static bool IsOpenSpace(Vector<int> point, int favorite)
            => (((point.X * point.X) + (3 * point.X) + (2 * point.X * point.Y) + point.Y + (point.Y * point.Y)) + favorite)
                .ToBinary()
                .Count(x => x == '1') % 2 == 0;

        private static VectorDictionary<int, int> CreateMap(int favorite, Vector<int> target)
        {
            VectorDictionary<int, int> map = new();

            Vector<int>.AxisEnumerator(target.X, target.Y).ForEach(point =>
            {
                if (IsOpenSpace(point, favorite))
                {
                    map.Add(point, 1);
                }
            });

            return map;
        }

        private void FindPath(Vector<int> point, int steps, HashSet<Vector<int>> visited)
        {
            visited.Add(point);

            if (point == this.Target)
            {
                this.Paths.Add(steps);
                return;
            }

            if (steps <= 50)
            {
                foreach (Vector<int> visit in visited)
                {
                    if (!this.Visited.Contains(visit))
                    {
                        this.Visited.Add(visit);
                    }
                }
            }

            steps++;

            foreach (VectorCell<int, int> move in this.Map.AdjacentCardinal(point).Where(cell => !visited.Contains(cell.Point)))
            {
                this.FindPath(move.Point, steps, new(visited));
            }
        }
    }
}
