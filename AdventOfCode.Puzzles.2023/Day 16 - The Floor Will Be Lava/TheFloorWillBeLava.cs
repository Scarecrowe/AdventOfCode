namespace AdventOfCode.Puzzles._2023.Day_16___The_Floor_Will_Be_Lava
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TheFloorWillBeLava
    {
        public TheFloorWillBeLava(string[] input)
        {
            this.Map = new(input, (c) => c);
        }

        private VectorArray<int, char> Map { get; }

        public int Shine()
        {
            List<int> results = new();

            foreach(var cell in this.Map.EdgeEnumerator())
            {
                if (cell.Point == new Vector<int>(0, 0))
                {
                    results.Add(this.Beam(cell.Point, Cardinal.East));
                    results.Add(this.Beam(cell.Point, Cardinal.South));
                    continue;
                }

                if (cell.Point == new Vector<int>(0, this.Map.Width - 1))
                {
                    results.Add(this.Beam(cell.Point, Cardinal.West));
                    results.Add(this.Beam(cell.Point, Cardinal.South));
                    continue;
                }

                if (cell.Point == new Vector<int>(this.Map.Height - 1, 0))
                {
                    results.Add(this.Beam(cell.Point, Cardinal.West));
                    results.Add(this.Beam(cell.Point, Cardinal.North));
                    continue;
                }

                if (cell.Point == new Vector<int>(this.Map.Height - 1, this.Map.Width - 1))
                {
                    results.Add(this.Beam(cell.Point, Cardinal.East));
                    results.Add(this.Beam(cell.Point, Cardinal.North));
                    continue;
                }

                if (cell.Point.Y == 0)
                {
                    results.Add(this.Beam(cell.Point, Cardinal.South));
                    continue;
                }

                if (cell.Point.Y == this.Map.Height - 1)
                {
                    results.Add(this.Beam(cell.Point, Cardinal.North));
                    continue;
                }

                if (cell.Point.X == 0)
                {
                    results.Add(this.Beam(cell.Point, Cardinal.East));
                    continue;
                }

                if (cell.Point.X == this.Map.Width - 1)
                {
                    results.Add(this.Beam(cell.Point, Cardinal.West));
                    continue;
                }
            }

            return results.Max();
        }

        public int Beam(Vector<int> start, Cardinal direction)
        {
            Queue<(Vector<int> Point, Cardinal Direction)> queue = new();
            queue.Enqueue((start.Clone(), direction));

            HashSet<(Vector<int>, Cardinal)> visited = new();

            while(queue.Any())
            {
                var current = queue.Dequeue();

                if (visited.Contains((current.Point, current.Direction))
                    || (current.Point.X < 0 || current.Point.X >= this.Map.Width)
                    || (current.Point.Y < 0 || current.Point.Y >= this.Map.Height))
                {
                    continue;
                }

                visited.Add((current.Point, current.Direction));

                switch (this.Map[current.Point])
                {
                    case '.':
                        queue.Enqueue((current.Point.Clone().Transform(current.Direction), current.Direction));
                        break;
                    case '/':
                        switch (current.Direction)
                        {
                            case Cardinal.North:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.East), Cardinal.East));
                                break;
                            case Cardinal.South:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.West), Cardinal.West));
                                break;
                            case Cardinal.East:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.North), Cardinal.North));
                                break;
                            case Cardinal.West:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.South), Cardinal.South));
                                break;
                        }

                        break;
                    case '\\':
                        switch (current.Direction)
                        {
                            case Cardinal.North:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.West), Cardinal.West));
                                break;
                            case Cardinal.South:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.East), Cardinal.East));
                                break;
                            case Cardinal.East:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.South), Cardinal.South));
                                break;
                            case Cardinal.West:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.North), Cardinal.North));
                                break;
                        }

                        break;
                    case '|':
                        switch (current.Direction)
                        {
                            case Cardinal.North:
                            case Cardinal.South:
                                queue.Enqueue((current.Point.Clone().Transform(current.Direction), current.Direction));
                                break;
                            case Cardinal.East:
                            case Cardinal.West:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.North), Cardinal.North));
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.South), Cardinal.South));
                                break;
                        }

                        break;
                    case '-':
                        switch (current.Direction)
                        {
                            case Cardinal.North:
                            case Cardinal.South:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.East), Cardinal.East));
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.West), Cardinal.West));
                                break;
                            case Cardinal.East:
                            case Cardinal.West:
                                queue.Enqueue((current.Point.Clone().Transform(current.Direction), current.Direction));
                                break;
                        }

                        break;
                }
            }

            return visited.Select(x => x.Item1).Distinct().Count();
        }
    }
}
