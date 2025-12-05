namespace AdventOfCode.Puzzles._2023.Day_22___Sand_Slabs
{
    using System.Collections.Generic;
    using System.Linq;

    public class SandSlabs
    {
        private List<Brick> Bricks { get; set; }

        private Dictionary<(int x, int y), (Brick brick, int topZ)> Heightmap { get; set; } = new();

        public SandSlabs(string[] input)
        {
            this.Bricks = input
                .Select(line =>
                {
                    var c = line.Split('~', ',')
                                .Select(int.Parse).ToArray();
                    return new Brick
                    {
                        X1 = c[0],
                        Y1 = c[1],
                        Z1 = c[2],
                        X2 = c[3],
                        Y2 = c[4],
                        Z2 = c[5]
                    };
                })
                .OrderBy(b => b.Z1)
                .ToList();

            this.Settle();
        }

        public int Fall() => this.Bricks.Count(b => b.Above.All(a => a.Supporters.Count > 1));

        public int Disintergrate()
        {
            int total = 0;

            foreach (Brick brick in this.Bricks)
            {
                HashSet<Brick> removed = new() { brick };
                Queue<Brick> queue = new();
                queue.Enqueue(brick);

                while (queue.TryDequeue(out var current))
                {
                    foreach (Brick above in current.Above)
                    {
                        if (above.Supporters.All(removed.Contains))
                        {
                            if (removed.Add(above))
                            {
                                total++;
                                queue.Enqueue(above);
                            }
                        }
                    }
                }
            }

            return total;
        }

        private void Settle()
        {
            foreach (Brick brick in this.Bricks)
            {
                brick.Above.Clear();
                brick.Supporters.Clear();
            }

            this.Heightmap.Clear();

            foreach (var brick in Bricks)
            {
                int maxBelow = 0;
                Dictionary<Brick, int> candidateSupporters = new();

                foreach (var (x, y) in brick.Coords)
                {
                    if (Heightmap.TryGetValue((x, y), out var cell))
                    {
                        if (cell.topZ > maxBelow)
                        {
                            maxBelow = cell.topZ;
                        }

                        if (candidateSupporters.TryGetValue(cell.brick, out var recordedTop))
                        {
                            if (cell.topZ > recordedTop)
                            {
                                candidateSupporters[cell.brick] = cell.topZ;
                            }
                        }
                        else
                        {
                            candidateSupporters[cell.brick] = cell.topZ;
                        }
                    }
                }

                int newZ1 = maxBelow + 1;
                int newZ2 = newZ1 + brick.Height - 1;

                brick.Z1 = newZ1;
                brick.Z2 = newZ2;

                if (maxBelow > 0)
                {
                    foreach (var kv in candidateSupporters)
                    {
                        Brick candidate = kv.Key;
                        int topZOfCandidate = kv.Value;

                        if (topZOfCandidate == maxBelow)
                        {
                            candidate.Above.Add(brick);
                            brick.Supporters.Add(candidate);
                        }
                    }
                }

                foreach (var (x, y) in brick.Coords)
                {
                    this.Heightmap[(x, y)] = (brick, newZ2);
                }
            }
        }
    }
}
