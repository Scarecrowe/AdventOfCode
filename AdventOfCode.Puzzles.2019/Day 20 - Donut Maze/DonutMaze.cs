namespace AdventOfCode.Puzzles._2019.Day_20___Donut_Maze
{
    using AdventOfCode.Core;

    public class DonutMaze
    {
        public DonutMaze(string[] input)
        {
            this.Map = new(input, (c) => c);
            this.Portals = this.GetPortals().ToList();
            this.Start = this.Portals.First(c => c.Name == "AA");
        }

        private VectorArray<int, char> Map { get; set; }

        private List<Portal> Portals { get; set; }

        private Portal Start { get; }

        public int Search(bool recursive = false)
        {
            List<(List<Portal> Portals, bool[,] Visited)> states = new() { (this.Portals, new bool[this.Map.Height, this.Map.Width]) };
            Queue<(Vector<int> Point, int Distance, int Step)> queue = new();
            this.Start.Travelled = true;

            queue.Enqueue((new(this.Start.Point), 0, 0));

            while (queue.Any())
            {
                (Vector<int> point, int distance, int step) = queue.Dequeue();

                if (states[recursive ? step : 0].Visited[point.Y, point.X])
                {
                    continue;
                }

                states[recursive ? step : 0].Visited[point.Y, point.X] = true;

                foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(point))
                {
                    if (adjacent.Value == '.')
                    {
                        queue.Enqueue((new(adjacent.Point), distance + 1, step));
                    }
                    else if (char.IsLetter(adjacent.Value))
                    {
                        Portal? start = states[recursive ? step : 0].Portals.First(c => c.Point == point);

                        if (start.Travelled)
                        {
                            continue;
                        }

                        start.Travelled = true;

                        if (recursive)
                        {
                            if (step > 0 && (start.Name == "AA" || start.Name == "ZZ"))
                            {
                                continue;
                            }

                            if (start.Name == "ZZ")
                            {
                                return distance;
                            }

                            if (step == 0 && !start.Inner)
                            {
                                continue;
                            }
                        }

                        if (start.Name == "ZZ")
                        {
                            return distance;
                        }

                        Portal? end = states[recursive ? step : 0].Portals.First(c => c.Name == start.Name && c != start);

                        if (recursive
                            && start.Inner
                            && states.Count == step + 1)
                        {
                            List<Portal> portals = new();

                            foreach (Portal portal in this.Portals)
                            {
                                portals.Add(new(portal));
                            }

                            states.Add((portals, new bool[this.Map.Height, this.Map.Width]));
                        }

                        queue.Enqueue((end.Point, distance + 1, step + (start.Inner ? 1 : -1)));
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private List<Portal> GetPortals()
        {
            List<Portal> portals = new();
            List<ProcessedCell<int, char>> cells = this.Map.Letters().Select(c => new ProcessedCell<int, char>(c)).ToList();

            foreach (ProcessedCell<int, char> cellA in cells.Where(c => !c.Processed))
            {
                List<VectorCell<int, char>> adjacentA = this.Map.AdjacentCardinal(cellA.Point).ToList();
                VectorCell<int, char>? portal = adjacentA.FirstOrDefault(c => c.Value == '.');

                foreach (ProcessedCell<int, char> cellB in cells.Where(c => !c.Processed))
                {
                    if (cellA == cellB)
                    {
                        continue;
                    }

                    if (adjacentA.Any(c => c.Point == cellB.Point))
                    {
                        if (portal != default)
                        {
                            portals.Add(new(portal.Point, $"{cellA.Value}{cellB.Value}", !(portal.Point.X == 2 || portal.Point.Y == 2 || portal.Point.X == this.Map.Width - 3 || portal.Point.Y == this.Map.Height - 3)));
                        }
                        else
                        {
                            portal = this.Map.AdjacentCardinal(cellB.Point).FirstOrDefault(c => c.Value == '.');
                            portals.Add(new(portal?.Point ?? new(0, 0), $"{cellA.Value}{cellB.Value}", !((portal?.Point.X ?? 0) == 2 || (portal?.Point.Y ?? 0) == 2 || (portal?.Point.X ?? 0) == this.Map.Width - 3 || (portal?.Point.Y ?? 0) == this.Map.Height - 3)));
                        }

                        cellA.Processed = true;
                        cellB.Processed = true;
                    }
                }
            }

            return portals;
        }
    }
}
