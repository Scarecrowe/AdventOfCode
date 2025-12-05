namespace AdventOfCode.Puzzles._2023.Day_18___Lavaduct_Lagoon
{
    public class LavaductLagoon(string[] input)
    {
        private HashSet<(long x, long y)> smallTrench = new HashSet<(long x, long y)>();

        private Dictionary<long, List<(long start, long end)>> rowIntervals = new Dictionary<long, List<(long start, long end)>>();

        private List<(Direction Dir, long Distance)> instructions = InstructionParser.ParseNormalInstructions(input);

        private List<(Direction Dir, long Distance)> hexInstructions = InstructionParser.ParseHexInstructions(input);

        public LavaductLagoon DigSmall(List<(Direction Dir, long Distance)> instructions)
        {
            long x = 0, y = 0;
            smallTrench.Add((x, y));

            foreach (var inst in instructions)
            {
                for (long i = 0; i < inst.Distance; i++)
                {
                    switch (inst.Dir)
                    {
                        case Direction.R: x++; break;
                        case Direction.L: x--; break;
                        case Direction.U: y++; break;
                        case Direction.D: y--; break;
                    }
                    smallTrench.Add((x, y));
                }
            }

            return this;
        }

        public LavaductLagoon DigLarge(List<(Direction Dir, long Distance)> instructions)
        {
            long x = 0, y = 0;
            AddInterval(y, x, x);

            foreach (var inst in instructions)
            {
                switch (inst.Dir)
                {
                    case Direction.R:
                        AddInterval(y, x + 1, x + inst.Distance);
                        x += inst.Distance;
                        break;
                    case Direction.L:
                        AddInterval(y, x - inst.Distance, x - 1);
                        x -= inst.Distance;
                        break;
                    case Direction.U:
                        for (long dy = 1; dy <= inst.Distance; dy++)
                            AddInterval(y + dy, x, x);
                        y += inst.Distance;
                        break;
                    case Direction.D:
                        for (long dy = 1; dy <= inst.Distance; dy++)
                            AddInterval(y - dy, x, x);
                        y -= inst.Distance;
                        break;
                }
            }

            return this;
        }

        private void AddInterval(long y, long start, long end)
        {
            if (!rowIntervals.ContainsKey(y))
            {
                rowIntervals[y] = new List<(long start, long end)>();
            }

            if (start > end) (start, end) = (end, start);
            {
                rowIntervals[y].Add((start, end));
            }
        }

        private List<(long start, long end)> MergeIntervals(List<(long start, long end)> intervals)
        {
            if (intervals.Count == 0)
            {
                return new List<(long start, long end)>();
            }

            intervals.Sort((a, b) => a.start.CompareTo(b.start));
            var merged = new List<(long start, long end)>();
            var current = intervals[0];

            for (int i = 1; i < intervals.Count; i++)
            {
                if (intervals[i].start <= current.end + 1)
                {
                    current.end = Math.Max(current.end, intervals[i].end);
                }
                else
                {
                    merged.Add(current);
                    current = intervals[i];
                }
            }

            merged.Add(current);

            return merged;
        }

        public long ComputeSmallVolume()
        {
            long minX = smallTrench.Min(p => p.x) - 1;
            long maxX = smallTrench.Max(p => p.x) + 1;
            long minY = smallTrench.Min(p => p.y) - 1;
            long maxY = smallTrench.Max(p => p.y) + 1;

            var visited = new HashSet<(long x, long y)>();
            var queue = new Queue<(long x, long y)>();
            queue.Enqueue((minX, minY));
            visited.Add((minX, minY));

            var dirs = new (long dx, long dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

            var outside = new HashSet<(long x, long y)>();

            while (queue.Count > 0)
            {
                var pos = queue.Dequeue();
                outside.Add(pos);

                foreach (var d in dirs)
                {
                    var next = (pos.x + d.dx, pos.y + d.dy);

                    if (next.Item1 < minX || next.Item1 > maxX || next.Item2 < minY || next.Item2 > maxY)
                    {
                        continue;
                    }

                    if (smallTrench.Contains(next))
                    {
                        continue;
                    }

                    if (visited.Contains(next))
                    {
                        continue;
                    }

                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }

            long total = 0;

            for (long x = minX + 1; x < maxX; x++)
            {
                for (long y = minY + 1; y < maxY; y++)
                {
                    if (!smallTrench.Contains((x, y)) && !outside.Contains((x, y)))
                    {
                        total++;
                    }
                }                    
            }
                
            return total + smallTrench.Count;
        }

        public long ComputeLargeVolume()
        {
            long total = 0;
            foreach (var kvp in rowIntervals)
            {
                var intervals = MergeIntervals(kvp.Value);
                foreach (var (start, end) in intervals)
                {
                    total += end - start + 1;
                }
            }
            return total;
        }

        public long Small() => DigSmall(this.instructions).ComputeSmallVolume();

        public long Large() => DigLarge(this.hexInstructions).ComputeLargeVolume();
    }
}
