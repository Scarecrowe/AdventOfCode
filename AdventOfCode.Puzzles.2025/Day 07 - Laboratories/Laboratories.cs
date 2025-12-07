namespace AdventOfCode.Puzzles._2025.Day_07___Laboratories
{
    using AdventOfCode.Core;

    public class Laboratories
    {
        public VectorArray<long, char> Map { get; private set; }

        public Vector<long> Start { get; private set; }

        public Laboratories(string[] input)
        {
            this.Map = new(input, (c) => c);
            this.Start = this.Map.AxisEnumerator().First(x => x.Value == 'S').Point;
        }

        public long SplitBeams()
        {
            Queue<Vector<long>> queue = new();
            queue.Enqueue(this.Start);

            long result = 0;

            while (queue.Count > 0)
            {
                Vector<long> current = queue.Dequeue();
                IEnumerable<VectorCell<long, char>> adjacent = this.Map.AdjacentCardinal(current);
                VectorCell<long, char>? south = adjacent.FirstOrDefault(x => x.Direction == Cardinal.South);

                if (south == null)
                {
                    continue;
                }

                if (south.Value == '.')
                {
                    this.Map[south.Point] = '|';
                    queue.Enqueue(south.Point);
                }

                if (south.Value == '^')
                {
                    result++;
                    adjacent = this.Map.AdjacentCardinal(current);
                    VectorCell<long, char>? west = adjacent.FirstOrDefault(x => x.Direction == Cardinal.West);
                    VectorCell<long, char>? east = adjacent.FirstOrDefault(x => x.Direction == Cardinal.East);

                    if (west != null)
                    {
                        this.Map[west.Point] = '|';
                        queue.Enqueue(west.Point);
                    }

                    if (east != null)
                    {
                        this.Map[east.Point] = '|';
                        queue.Enqueue(east.Point);
                    }                 
                }
            }

            return result;
        }

        public long Timelines()
        {
            Dictionary<Vector<long>, long> timelines = [];
            timelines[this.Start] = 1;

            Queue<Vector<long>> queue = new();
            queue.Enqueue(this.Start);

            while (queue.Count > 0)
            {
                Vector<long> point = queue.Dequeue();
                long count = timelines[point];

                VectorCell<long, char>? south = this.Map.AdjacentCardinal(point)
                    .FirstOrDefault(a => a.Direction == Cardinal.South);

                if (south == null)
                {
                    continue;
                }

                if (south.Value == '.')
                {
                    if (!timelines.ContainsKey(south.Point))
                    {
                        queue.Enqueue(south.Point);
                    }

                    timelines[south.Point] = timelines.GetValueOrDefault(south.Point) + count;
                    continue;
                }

                if (south.Value == '^')
                {
                    IEnumerable<VectorCell<long, char>> adjacent = this.Map.AdjacentCardinal(south.Point);

                    VectorCell<long, char>? west = adjacent.FirstOrDefault(x => x.Direction == Cardinal.West);
                    VectorCell<long, char>? east = adjacent.FirstOrDefault(x => x.Direction == Cardinal.East);

                    if (west != null && west.Value == '.')
                    {
                        if (!timelines.ContainsKey(west.Point))
                        {
                            queue.Enqueue(west.Point);
                        }

                        timelines[west.Point] = timelines.GetValueOrDefault(west.Point) + count;
                    }

                    if (east != null && east.Value == '.')
                    {
                        if (!timelines.ContainsKey(east.Point))
                        {
                            queue.Enqueue(east.Point);
                        }

                        timelines[east.Point] = timelines.GetValueOrDefault(east.Point) + count;
                    }
                }
            }

            long result = 0;

            foreach (KeyValuePair<Vector<long>, long> timeline in timelines)
            {
                Vector<long> point = timeline.Key;
                VectorCell<long, char>? south = this.Map.AdjacentCardinal(point).FirstOrDefault(x => x.Direction == Cardinal.South);
                bool terminal;

                if (south == null)
                {
                    terminal = true;
                }
                else if (south.Value != '.' && south.Value != '^')
                {
                    terminal = true;
                }                    
                else
                {
                    terminal = false;
                }

                if (terminal)
                {
                    result += timeline.Value;
                }
            }

            return result;
        }
    }
}
