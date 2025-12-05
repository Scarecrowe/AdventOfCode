namespace AdventOfCode.Puzzles._2024.Day_06___Guard_Gallivant
{
    using AdventOfCode.Core;
    using System.Collections.Generic;

    public class GuardGallivant
    {
        public VectorArray<int, char> Map { get; private set; }

        public Vector<int> Start { get; private set; }

        public GuardGallivant(string[] input)
        {
            this.Map = new(input, c => c);
            this.Start = this.Map.AxisEnumerator().First(x => x.Value == '^').Point;
            this.Map[this.Start] = '.';
        }

        private int Patrol(Vector<int>? collision = null)
        {
            Queue<(Vector<int> Guard, Vector<int>? Collision, Cardinal Direction)> queue = new();
            HashSet<(Vector<int> Point, Cardinal Direction)> visited = [(this.Start.Clone(), Cardinal.North)];

            queue.Enqueue((this.Start.Clone(), collision, Cardinal.North));

            while (queue.Count != 0)
            {
                var state = queue.Dequeue();
                Vector<int> moved = state.Guard.Clone().Transform(state.Direction);

                if (!this.Map.Contains(moved))
                {
                    return collision == null ? Distinct(visited) : 0;
                }

                switch (collision == null || collision != moved ? this.Map[moved] : '#')
                {
                    case '#':
                        switch (state.Direction)
                        {
                            case Cardinal.North:
                                state.Direction = Cardinal.East;
                                break;
                            case Cardinal.South:
                                state.Direction = Cardinal.West;
                                break;
                            case Cardinal.East:
                                state.Direction = Cardinal.South;
                                break;
                            case Cardinal.West:
                                state.Direction = Cardinal.North;
                                break;
                        }

                        break;
                    case '.':
                        if (visited.Contains((moved, state.Direction)))
                        {
                            return 1;
                        }

                        visited.Add((moved.Clone(), state.Direction));
                        state.Guard = moved.Clone();
                        break;
                }

                queue.Enqueue((state.Guard, collision, state.Direction));
            }

            return 0;
        }

        private static int Distinct(HashSet<(Vector<int> Point, Cardinal Direction)> visited) => visited.Select(x => x.Point).Distinct().Count();
    
        public int WithoutCollisions() => this.Patrol();

        public int WithCollisions()
        {
            int result = 0;

            foreach(var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value == '#'
                    || cell.Point == this.Start)
                {
                    continue;
                }

                if (this.Patrol(cell.Point) > 0)
                {
                    result++;
                }
            }

            return result;
        }
    }
}
