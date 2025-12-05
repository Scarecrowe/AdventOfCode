namespace AdventOfCode.Puzzles._2024.Day_16___Reindeer_Maze
{
    using AdventOfCode.Core;

    public class ReindeerMaze
    {
        public VectorArray<int, char> Map { get; private set; }

        public Vector<int> Reindeer { get; private set; }

        public Vector<int> Finish { get; private set; }

        public ReindeerMaze(string[] input)
        {
            this.Map = new VectorArray<int, char>(input, c => c);

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value == 'S')
                {
                    this.Reindeer = cell.Point.Clone();
                    continue;
                }

                if (cell.Value == 'E')
                {
                    this.Finish = cell.Point.Clone();
                }
            }
        }

        public int BestScore() => this.ShortestPath().Score;

        public int Seats()
        {
            var shortestPath = this.ShortestPath();
            HashSet<Vector<int>> seats = [];
            
            HashSet<(Vector<int>, Cardinal)> visited = [];
            PriorityQueue<(Vector<int> Point, Cardinal Direction, ReindeerPath Path), int> queue = new();

            queue.Enqueue((this.Reindeer, Cardinal.East, new(1)), 1);
            queue.Enqueue((this.Reindeer, CardinalHelper.Clockwise[Cardinal.East], new(1000)), 1000);
            queue.Enqueue((this.Reindeer, CardinalHelper.AntiClockwise[Cardinal.East], new(1000)), 1000);

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();

                if (visited.Contains((state.Point, state.Direction)))
                {                  
                    continue;
                }

                visited.Add((state.Point, state.Direction));

                foreach (var cell in this.Map.AdjacentCardinal(state.Point))
                {
                    if (cell.Value == '.')
                    {
                        var path = state.Path.Clone();

                        if (cell.Direction == state.Direction)
                        {
                            path.Score += 1;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, state.Direction, path), path.Score);
                        }
                        else if (cell.Direction == CardinalHelper.Clockwise[state.Direction])
                        {
                            path.Score += 1001;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, CardinalHelper.Clockwise[state.Direction], path), path.Score);
                        }
                        else if (cell.Direction == CardinalHelper.AntiClockwise[state.Direction])
                        {
                            path.Score += 1001;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, CardinalHelper.AntiClockwise[state.Direction], path), path.Score);
                        }
                    }
                    else if (cell.Value == 'E')
                    {
                        state.Path.Score++;
                    }
                }
            }

            PuzzleConsole.WriteLine(this.Map.Print((c, p) =>
            {
                if (seats.Contains(p))
                {
                    return 'O';
                }

                return c;
            }));

            PuzzleConsole.Flush();

            return seats.Count;
        }

        public ReindeerPath ShortestPath()
        {
            int score = 0;
            ReindeerPath result = null;
            HashSet<(Vector<int>, Cardinal)> visited = [];
            PriorityQueue<(Vector<int> Point, Cardinal Direction, ReindeerPath Path), int> queue = new();

            queue.Enqueue((this.Reindeer, Cardinal.East, new(1)), 1);
            queue.Enqueue((this.Reindeer, CardinalHelper.Clockwise[Cardinal.East], new(1000)), 1000);
            queue.Enqueue((this.Reindeer, CardinalHelper.AntiClockwise[Cardinal.East], new(1000)), 1000);

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();

                if (visited.Contains((state.Point, state.Direction)))
                {
                    continue;
                }

                visited.Add((state.Point, state.Direction));

                foreach (var cell in this.Map.AdjacentCardinal(state.Point))
                {
                    if (cell.Value == '.')
                    {
                        var path = state.Path.Clone();

                        if (cell.Direction == state.Direction)
                        {
                            path.Score += 1;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, state.Direction, path), path.Score);
                        }
                        else if (cell.Direction == CardinalHelper.Clockwise[state.Direction])
                        {
                            path.Score += 1001;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, CardinalHelper.Clockwise[state.Direction], path), path.Score);
                        }
                        else if (cell.Direction == CardinalHelper.AntiClockwise[state.Direction])
                        {
                            path.Score += 1001;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, CardinalHelper.AntiClockwise[state.Direction], path), path.Score);
                        }
                    }
                    else if (cell.Value == 'E')
                    {
                        state.Path.Score++;

                        if (score == 0)
                        {
                            score = state.Path.Score;
                            result = state.Path;
                        }
                        else if (state.Path.Score < score)
                        {
                            score = state.Path.Score;
                            result = state.Path;
                        }
                    }
                }
            }

            return result;
        }
    }
}
