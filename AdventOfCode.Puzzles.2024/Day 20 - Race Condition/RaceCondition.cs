namespace AdventOfCode.Puzzles._2024.Day_20___Race_Condition
{
    using AdventOfCode.Core;

    public class RaceCondition
    {
        public VectorArray<int, char> Map { get; private set; }

        public Vector<int> Start { get; private set; }

        public Vector<int> End { get; private set; }

        private Dictionary<Vector<int>, int> Distance { get; set; }

        public RaceCondition(string[] input)
        {
            this.Map = new(input, c => c);
            this.Start = new(0, 0);
            this.End = new(0, 0);

            foreach (var cell in Map.AxisEnumerator())
            {
                if (cell.Value == 'S')
                {
                    this.Start = cell.Point;
                }
                else if (cell.Value == 'E')
                {
                    this.End = cell.Point;
                }
            }

            this.Map[this.Start] = '.';
            this.Map[this.End] = '.';

            this.Distance = this.ComputeDistanceFromEnd();
        }

        private Dictionary<Vector<int>, int> ComputeDistanceFromEnd()
        {
            Dictionary<Vector<int>, int> distance = new();
            Queue<Vector<int>> queue = new();

            queue.Enqueue(this.End);
            distance[this.End] = 0;

            while (queue.Count > 0)
            {
                Vector<int> point = queue.Dequeue();
                int current = distance[point];

                foreach (var cell in Map.AdjacentCardinal(point))
                {
                    if (cell.Value != '.')
                    {
                        continue;
                    }

                    if (distance.ContainsKey(cell.Point))
                    {
                        continue;
                    }

                    distance[cell.Point] = current + 1;
                    queue.Enqueue(cell.Point);
                }
            }

            return distance;
        }

        public int CountCheats(int range)
        {
            int result = 0;

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value != '.')
                {
                    continue;
                }

                var from = cell.Point;

                if (!this.Distance.TryGetValue(from, out int distFromEnd))
                {
                    continue;
                }

                for (int dr = -range; dr <= range; dr++)
                {
                    for (int dc = -range; dc <= range; dc++)
                    {
                        int man = Math.Abs(dr) + Math.Abs(dc);

                        if (man > range)
                        {
                            continue;
                        }

                        var to = new Vector<int>(from.X + dr, from.Y + dc);

                        if (!this.Map.IsVectorInRange(to))
                        {
                            continue;
                        }

                        if (this.Map[to] != '.')
                        {
                            continue;
                        }

                        if (!this.Distance.TryGetValue(to, out int distToEnd))
                        {
                            continue;
                        }

                        int saved = distFromEnd - distToEnd - man;

                        if (saved >= 100)
                        {
                            result++;
                        }
                    }
                }
            }

            return result;
        }

        public int NormalRace() => CountCheats(2);

        public int ExtendedRace() => CountCheats(20);
    }

}
