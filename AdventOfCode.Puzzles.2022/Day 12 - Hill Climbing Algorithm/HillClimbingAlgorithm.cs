namespace AdventOfCode.Puzzles._2022.Day_12___Hill_Climbing_Algorithm
{
    using AdventOfCode.Core;

    public class HillClimbingAlgorithm
    {
        public HillClimbingAlgorithm(string[] input, bool isStartOnly = true)
        {
            this.Positions = new();
            this.Moves = new();
            this.Finish = new(0, 0);
            this.Map = this.Parse(input, isStartOnly);
            this.Visited = new(this.Map.Width, this.Map.Height);
        }

        private VectorArray<int, int> Map { get; set; }

        private List<Vector<int>> Positions { get; set; }

        private Vector<int> Finish { get; set; }

        private VectorArray<int, bool> Visited { get; set; }

        private List<int> Moves { get; set; }

        public int Fewest()
        {
            foreach (Vector<int> position in this.Positions)
            {
                this.Visited = new(this.Map.Width, this.Map.Height);
                Queue<(Vector<int> Point, int Move)> queue = new();
                queue.Enqueue((position, 0));

                while (queue.Count > 0)
                {
                    (Vector<int> point, int move) = queue.Dequeue();
                    int elevation = this.Map.GetValue(point);

                    if (point.Y - 1 >= 0)
                    {
                        this.Enqueue(queue, point + new Vector<int>(0, -1), move, elevation);
                    }

                    if (point.Y + 1 < this.Map.Height)
                    {
                        this.Enqueue(queue, point + new Vector<int>(0, 1), move, elevation);
                    }

                    if (point.X - 1 >= 0)
                    {
                        this.Enqueue(queue, point + new Vector<int>(-1, 0), move, elevation);
                    }

                    if (point.X + 1 < this.Map.Width - 1)
                    {
                        this.Enqueue(queue, point + new Vector<int>(1, 0), move, elevation);
                    }
                }
            }

            return this.Moves.Min();
        }

        private VectorArray<int, int> Parse(string[] input, bool isStartOnly)
        {
            return new(input, (c, x, y) =>
            {
                char value = c;

                if (value == 'S')
                {
                    this.Positions.Add(new(x, y));
                    value = 'a';
                }

                if (value == 'E')
                {
                    this.Finish = new(x, y);
                    value = 'z';
                }

                if (!isStartOnly && value == 'a' && !this.Positions.Contains(new(x, y)))
                {
                    this.Positions.Add(new(x, y));
                }

                return value;
            });
        }

        private void Enqueue(Queue<(Vector<int> Point, int Move)> queue, Vector<int> point, int move, int elevation)
        {
            if (!this.Visited[point.Y, point.X] && this.Map[point.Y, point.X] <= elevation + 1)
            {
                this.Visited[point.Y, point.X] = true;

                if (point == this.Finish)
                {
                    this.Moves.Add(move + 1);
                }
                else
                {
                    queue.Enqueue((point, move + 1));
                }
            }
        }
    }
}
