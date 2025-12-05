namespace AdventOfCode.Puzzles._2024.Day_12___Garden_Groups
{
    using AdventOfCode.Core;

    public class GardenGroups
    {
        public VectorArray<int, char> Map { get; private set; }

        public GardenGroups(string[] input)
        {
            this.Map = new(input, c => c);
        }

        public int Cost()
        {
            HashSet<Vector<int>> visited = new();

            List<HashSet<Vector<int>>> regions = new();

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (visited.Contains(cell.Point))
                {
                    continue;
                }

                HashSet<Vector<int>> temp = new();
                Queue<Vector<int>> queue = new();
                queue.Enqueue(cell.Point);
                visited.Add(cell.Point);
                temp.Add(cell.Point);

                while (queue.Count > 0)
                {
                    var point = queue.Dequeue();

                    foreach (var adjacent in this.Map.AdjacentCardinal(point))
                    {
                        if (visited.Contains(adjacent.Point)
                            || cell.Value != adjacent.Value)
                        {
                            continue;
                        }

                        temp.Add(adjacent.Point);
                        visited.Add(adjacent.Point);
                        queue.Enqueue(adjacent.Point);
                    }
                }

                regions.Add(temp);
            }

            int result = 0;

            foreach (HashSet<Vector<int>> region in regions)
            {
                int perimeter = 0;
                visited.Clear();

                char value = this.Map[region.First()];

                foreach (Vector<int> point in region)
                {
                    var adjacent = this.Map.AdjacentCardinal(point).Where(x => x.Value == value && !visited.Contains(x.Point));

                    perimeter += (4 - (adjacent.Count() * 2));

                    visited.Add(point);
                }

                result += region.Count * perimeter;
            }

            return result;
        }

        public int Bulk()
        {
            HashSet<Vector<int>> visited = new();

            List<HashSet<Vector<int>>> regions = new();

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (visited.Contains(cell.Point))
                {
                    continue;
                }

                HashSet<Vector<int>> temp = new();
                Queue<Vector<int>> queue = new();
                queue.Enqueue(cell.Point);
                visited.Add(cell.Point);
                temp.Add(cell.Point);

                while (queue.Count > 0)
                {
                    var point = queue.Dequeue();

                    foreach (var adjacent in this.Map.AdjacentCardinal(point))
                    {
                        if (visited.Contains(adjacent.Point)
                            || cell.Value != adjacent.Value)
                        {
                            continue;
                        }

                        temp.Add(adjacent.Point);
                        visited.Add(adjacent.Point);
                        queue.Enqueue(adjacent.Point);
                    }
                }

                regions.Add(temp);
            }

            int result = 0;

            foreach (HashSet<Vector<int>> region in regions)
            {
                int perimeter = 0;
                visited.Clear();

                char value = this.Map[region.First()];

                foreach (Vector<int> point in region)
                {
                    var adjacent = this.Map.AdjacentCardinal(point).Where(x => x.Value == value);

                    perimeter += (4 - (adjacent.Count()));

                    visited.Add(point);
                }

                PuzzleConsole.WriteLine($"{value} -> {perimeter}");
                result += region.Count * perimeter;
            }

            return result;
        }
    }
}
