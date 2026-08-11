namespace AdventOfCode.Puzzles._2025.Day_07___Laboratories
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class Laboratories
    {
        public VectorArray<long, char> Map { get; private set; }

        public Vector<long> Start { get; private set; }

        public Laboratories(string[] input)
        {
            this.Map = new(input, (c) => c);
            this.Start = this.Map.AxisEnumerator().First(x => x.Value == 'S').Point;
        }

        public Laboratories(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct BeamState(Vector<long> Point, long Splits, long Active, long Step);

        private readonly record struct TimelineState(Vector<long> Point, long Count, long Active, long Complete, long Step);

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

        public Laboratories RenderSilver(int renderEvery = 4)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            HashSet<Vector<long>> beamTrail = [];
            HashSet<Vector<long>> activeBeams = [];
            HashSet<Vector<long>> splittersHit = [];
            Queue<Vector<long>> queue = new();

            queue.Enqueue(this.Start);
            beamTrail.Add(this.Start);

            long splits = 0;
            long step = 0;

            frames.Add(this.BuildFrame(
                beamTrail,
                activeBeams,
                splittersHit,
                new Dictionary<Vector<long>, long>(),
                $"LABORATORIES // BEAM SPLITTERS // SPLITS {splits:0000} // ACTIVE {queue.Count:0000}"));

            while (queue.Count > 0)
            {
                Vector<long> current = queue.Dequeue();
                activeBeams.Clear();
                step++;

                IEnumerable<VectorCell<long, char>> adjacent = this.Map.AdjacentCardinal(current);
                VectorCell<long, char>? south = adjacent.FirstOrDefault(x => x.Direction == Cardinal.South);

                if (south == null)
                {
                    if (step % renderEvery == 0)
                    {
                        frames.Add(this.BuildFrame(
                            beamTrail,
                            activeBeams,
                            splittersHit,
                            new Dictionary<Vector<long>, long>(),
                            $"LABORATORIES // BEAM EXITED // SPLITS {splits:0000} // ACTIVE {queue.Count:0000}"));
                    }

                    continue;
                }

                if (south.Value == '.')
                {
                    beamTrail.Add(south.Point);
                    activeBeams.Add(south.Point);
                    queue.Enqueue(south.Point);
                }
                else if (south.Value == '^')
                {
                    splits++;
                    splittersHit.Add(south.Point);

                    IEnumerable<VectorCell<long, char>> splitAdjacent = this.Map.AdjacentCardinal(south.Point);
                    VectorCell<long, char>? west = splitAdjacent.FirstOrDefault(x => x.Direction == Cardinal.West);
                    VectorCell<long, char>? east = splitAdjacent.FirstOrDefault(x => x.Direction == Cardinal.East);

                    if (west != null)
                    {
                        beamTrail.Add(west.Point);
                        activeBeams.Add(west.Point);
                        queue.Enqueue(west.Point);
                    }

                    if (east != null)
                    {
                        beamTrail.Add(east.Point);
                        activeBeams.Add(east.Point);
                        queue.Enqueue(east.Point);
                    }
                }

                if (step % renderEvery == 0 || queue.Count == 0)
                {
                    frames.Add(this.BuildFrame(
                        beamTrail,
                        activeBeams,
                        splittersHit,
                        new Dictionary<Vector<long>, long>(),
                        $"LABORATORIES // BEAM SPLITTERS // SPLITS {splits:0000} // ACTIVE {queue.Count:0000}"));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    beamTrail,
                    activeBeams,
                    splittersHit,
                    new Dictionary<Vector<long>, long>(),
                    $"LABORATORIES // REPAIR DATA READY // SPLITS {splits:0000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public Laboratories RenderGold(int renderEvery = 4)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            Dictionary<Vector<long>, long> timelines = [];
            HashSet<Vector<long>> quantumTrail = [];
            HashSet<Vector<long>> activeParticles = [];
            HashSet<Vector<long>> splittersHit = [];
            Queue<Vector<long>> queue = new();
            List<string[]> frames = [];

            timelines[this.Start] = 1;
            quantumTrail.Add(this.Start);
            queue.Enqueue(this.Start);

            long completed = 0;
            long step = 0;

            frames.Add(this.BuildFrame(
                quantumTrail,
                activeParticles,
                splittersHit,
                timelines,
                $"LABORATORIES // MANY WORLDS // TIMELINES {completed:00000000000000} // ACTIVE {queue.Count:0000}"));

            while (queue.Count > 0)
            {
                Vector<long> point = queue.Dequeue();
                long count = timelines[point];
                activeParticles.Clear();
                step++;

                VectorCell<long, char>? south = this.Map.AdjacentCardinal(point)
                    .FirstOrDefault(a => a.Direction == Cardinal.South);

                if (south == null)
                {
                    completed += count;
                }
                else if (south.Value == '.')
                {
                    this.EnqueueTimeline(south.Point, count, timelines, queue);
                    quantumTrail.Add(south.Point);
                    activeParticles.Add(south.Point);
                }
                else if (south.Value == '^')
                {
                    splittersHit.Add(south.Point);

                    IEnumerable<VectorCell<long, char>> adjacent = this.Map.AdjacentCardinal(south.Point);

                    VectorCell<long, char>? west = adjacent.FirstOrDefault(x => x.Direction == Cardinal.West);
                    VectorCell<long, char>? east = adjacent.FirstOrDefault(x => x.Direction == Cardinal.East);

                    if (west != null && west.Value == '.')
                    {
                        this.EnqueueTimeline(west.Point, count, timelines, queue);
                        quantumTrail.Add(west.Point);
                        activeParticles.Add(west.Point);
                    }

                    if (east != null && east.Value == '.')
                    {
                        this.EnqueueTimeline(east.Point, count, timelines, queue);
                        quantumTrail.Add(east.Point);
                        activeParticles.Add(east.Point);
                    }
                }
                else
                {
                    completed += count;
                }

                if (step % renderEvery == 0 || queue.Count == 0)
                {
                    long active = queue.Sum(p => timelines[p]);

                    frames.Add(this.BuildFrame(
                        quantumTrail,
                        activeParticles,
                        splittersHit,
                        timelines,
                        $"LABORATORIES // MANY WORLDS // TIMELINES {completed:00000000000000} // ACTIVE {active:00000000000000}"));
                }
            }

            long final = completed == 0
                ? this.CalculateTerminalTimelines(timelines)
                : completed;

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    quantumTrail,
                    activeParticles,
                    splittersHit,
                    timelines,
                    $"LABORATORIES // QUANTUM MANIFOLD COMPLETE // TIMELINES {final:00000000000000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private void EnqueueTimeline(
            Vector<long> point,
            long count,
            Dictionary<Vector<long>, long> timelines,
            Queue<Vector<long>> queue)
        {
            if (!timelines.ContainsKey(point))
            {
                queue.Enqueue(point);
            }

            timelines[point] = timelines.GetValueOrDefault(point) + count;
        }

        private long CalculateTerminalTimelines(Dictionary<Vector<long>, long> timelines)
        {
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

        private string[] BuildFrame(
            HashSet<Vector<long>> trail,
            HashSet<Vector<long>> active,
            HashSet<Vector<long>> splittersHit,
            Dictionary<Vector<long>, long> timelines,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (long y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (long x = 0; x < this.Map.Width; x++)
                {
                    Vector<long> point = new(x, y);
                    char value = this.Map[point];

                    if (active.Contains(point))
                    {
                        sb.Append('@');
                    }
                    else if (splittersHit.Contains(point))
                    {
                        sb.Append('*');
                    }
                    else if (timelines.TryGetValue(point, out long count) && count > 1)
                    {
                        sb.Append(GetTimelineCharacter(count));
                    }
                    else if (trail.Contains(point) && value == '.')
                    {
                        sb.Append('|');
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static char GetTimelineCharacter(long count)
        {
            if (count >= 1_000_000_000_000)
            {
                return 'Z';
            }

            if (count >= 1_000_000_000)
            {
                return 'B';
            }

            if (count >= 1_000_000)
            {
                return 'M';
            }

            if (count >= 1_000)
            {
                return 'K';
            }

            if (count >= 100)
            {
                return 'H';
            }

            if (count >= 10)
            {
                return '+';
            }

            return '|';
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(
                    new Frame(PadFrame(frame, width, height)));
            }
        }

        private static string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }
    }
}
