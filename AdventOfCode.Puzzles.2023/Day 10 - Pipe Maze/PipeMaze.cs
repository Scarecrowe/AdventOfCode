namespace AdventOfCode.Puzzles._2023.Day_10___Pipe_Maze
{
    using AdventOfCode.Core;

    public class PipeMaze
    {
        public PipeMaze(string[] input)
        {
            this.Map = new(input, (value) => value);
        }

        private VectorArray<int, char> Map { get; }

        public long Move()
        {
            Queue<(Vector<int> Point, Cardinal Direction, int Steps)> queue = new();

            foreach(var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value == 'S')
                {
                    foreach (var adjacent in this.Map.AdjacentCardinal(cell.Point))
                    {
                        if (adjacent.Value == '|'
                            && (adjacent.Direction == Cardinal.North
                                || adjacent.Direction == Cardinal.South))
                        {
                            queue.Enqueue((cell.Point, adjacent.Direction, 0));
                        }

                        if (adjacent.Value == '-'
                            && (adjacent.Direction == Cardinal.West
                                || adjacent.Direction == Cardinal.East))
                        {
                            queue.Enqueue((cell.Point, adjacent.Direction, 0));
                        }

                        if (adjacent.Value == '7'
                            && (adjacent.Direction == Cardinal.West
                                || adjacent.Direction == Cardinal.South))
                        {
                            queue.Enqueue((cell.Point, adjacent.Direction, 0));
                        }

                        if (adjacent.Value == 'L'
                             && (adjacent.Direction == Cardinal.East
                                 || adjacent.Direction == Cardinal.North))
                        {
                            queue.Enqueue((cell.Point, adjacent.Direction, 0));
                        }

                        if (adjacent.Value == 'J'
                             && (adjacent.Direction == Cardinal.West
                                 || adjacent.Direction == Cardinal.North))
                        {
                            queue.Enqueue((cell.Point, adjacent.Direction, 0));
                        }

                        if (adjacent.Value == 'F'
                             && (adjacent.Direction == Cardinal.East
                                 || adjacent.Direction == Cardinal.South))
                        {
                            queue.Enqueue((cell.Point, adjacent.Direction, 0));
                        }
                    }
                }
            }

            HashSet<Vector<int>> visited = new();

            long result = 0;

            while (queue.Any())
            {
                (Vector<int> Point, Cardinal Direction, int Steps) current = queue.Dequeue();

                if (visited.Contains(current.Point))
                {
                    continue;
                }

                visited.Add(current.Point);

                if (current.Steps > result)
                {
                    result = current.Steps;
                }

                foreach (var adjacent in this.Map.AdjacentCardinal(current.Point))
                {
                    if (adjacent.Value == '.')
                    {
                        continue;
                    }

                    if (adjacent.Value == '|'
                            && (adjacent.Direction == Cardinal.North
                                || adjacent.Direction == Cardinal.South))
                    {
                        queue.Enqueue((adjacent.Point, adjacent.Direction, current.Steps + 1));
                    }

                    if (adjacent.Value == '-'
                        && (adjacent.Direction == Cardinal.West
                            || adjacent.Direction == Cardinal.East))
                    {
                        queue.Enqueue((adjacent.Point, adjacent.Direction, current.Steps + 1));
                    }

                    if (adjacent.Value == '7'
                        && (adjacent.Direction == Cardinal.East
                            || adjacent.Direction == Cardinal.North))
                    {
                        queue.Enqueue((adjacent.Point, adjacent.Direction, current.Steps + 1));
                    }

                    if (adjacent.Value == 'L'
                         && (adjacent.Direction == Cardinal.West
                             || adjacent.Direction == Cardinal.South))
                    {
                        queue.Enqueue((adjacent.Point, adjacent.Direction, current.Steps + 1));
                    }

                    if (adjacent.Value == 'J'
                         && (adjacent.Direction == Cardinal.East
                             || adjacent.Direction == Cardinal.South))
                    {
                        queue.Enqueue((adjacent.Point, adjacent.Direction, current.Steps + 1));
                    }

                    if ((adjacent.Value == 'F'
                        || adjacent.Value == 'S')
                         && (adjacent.Direction == Cardinal.West
                             || adjacent.Direction == Cardinal.North))
                    {
                        queue.Enqueue((adjacent.Point, adjacent.Direction, current.Steps + 1));
                    }
                }
            }

            int count = 0;

            return count;
        }
    }
}
