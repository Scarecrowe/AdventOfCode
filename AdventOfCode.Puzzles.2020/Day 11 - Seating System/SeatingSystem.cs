namespace AdventOfCode.Puzzles._2020.Day_11___Seating_System
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class SeatingSystem
    {
        public SeatingSystem(string[] input) => this.Map = new(input, (chr) => (Entity)chr);

        public SeatingSystem(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<long, Entity> Map { get; set; }

        public long VisibleSeatCount()
        {
            Dictionary<Vector<long>, List<VectorCell<long, Entity>>> visible = this.VisibleSeats();

            while (true)
            {
                bool stateChanged = false;

                VectorArray<long, Entity> map = new(this.Map);

                this.Map.AxisEnumerator().Where(cell => cell.Value != Entity.Floor).ForEach(cell =>
                {
                    if (cell.Value == Entity.OccupiedSeat)
                    {
                        this.SetSeat(visible, cell, map, ref stateChanged, (value) => value >= 5, Entity.EmptySeat);
                    }
                    else
                    {
                        this.SetSeat(visible, cell, map, ref stateChanged, (value) => value == 0, Entity.OccupiedSeat);
                    }
                });

                if (!stateChanged)
                {
                    return this.Map.Count(Entity.OccupiedSeat);
                }

                this.Map = map;
            }
        }

        public long SeatCount()
        {
            while (true)
            {
                bool stateChanged = false;

                VectorArray<long, Entity> map = new(this.Map);

                foreach (VectorCell<long, Entity> cell in this.Map.AxisEnumerator())
                {
                    switch (cell.Value)
                    {
                        case Entity.OccupiedSeat:
                            List<VectorCell<long, Entity>> adjacent = this.Map.AdjacentInterCardinal(cell.Point).ToList();

                            if (adjacent.Count(x => x.Value == Entity.OccupiedSeat) >= 4)
                            {
                                map[cell.Point] = Entity.EmptySeat;
                                stateChanged = true;
                            }

                            break;
                        case Entity.EmptySeat:
                            adjacent = this.Map.AdjacentInterCardinal(cell.Point).ToList();

                            if (!adjacent.Any(x => x.Value == Entity.OccupiedSeat))
                            {
                                map[cell.Point] = Entity.OccupiedSeat;
                                stateChanged = true;
                            }

                            break;
                    }
                }

                if (!stateChanged)
                {
                    return this.Map.Count(Entity.OccupiedSeat);
                }

                this.Map = map;
            }
        }

        public SeatingSystem RenderSilver(int holdFrames = 24)
    => this.Render(false, holdFrames);

        public SeatingSystem RenderGold(int holdFrames = 24)
            => this.Render(true, holdFrames);

        private SeatingSystem Render(bool visibleMode, int holdFrames)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            Dictionary<Vector<long>, List<VectorCell<long, Entity>>>? visible =
                visibleMode ? this.VisibleSeats() : null;

            List<string[]> frames = [];
            int round = 0;

            frames.Add(this.BuildFrame(
                round,
                visibleMode ? "VISIBLE SEATING SYSTEM" : "ADJACENT SEATING SYSTEM",
                0));

            while (true)
            {
                bool stateChanged = false;
                VectorArray<long, Entity> map = new(this.Map);

                foreach (VectorCell<long, Entity> cell in this.Map.AxisEnumerator().Where(c => c.Value != Entity.Floor))
                {
                    int occupied = visibleMode
                        ? visible![cell.Point].Count(x => this.Map[x.Point] == Entity.OccupiedSeat)
                        : this.Map.AdjacentInterCardinal(cell.Point).Count(x => x.Value == Entity.OccupiedSeat);

                    if (cell.Value == Entity.EmptySeat && occupied == 0)
                    {
                        map[cell.Point] = Entity.OccupiedSeat;
                        stateChanged = true;
                    }
                    else if (cell.Value == Entity.OccupiedSeat && occupied >= (visibleMode ? 5 : 4))
                    {
                        map[cell.Point] = Entity.EmptySeat;
                        stateChanged = true;
                    }
                }

                if (!stateChanged)
                {
                    long occupied = this.Map.Count(Entity.OccupiedSeat);

                    for (int i = 0; i < holdFrames; i++)
                    {
                        frames.Add(this.BuildFrame(
                            round,
                            $"STABLE // OCCUPIED {occupied:0000}",
                            occupied));
                    }

                    break;
                }

                this.Map = map;
                round++;

                frames.Add(this.BuildFrame(
                    round,
                    visibleMode ? "VISIBLE SEATING SYSTEM" : "ADJACENT SEATING SYSTEM",
                    this.Map.Count(Entity.OccupiedSeat)));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private string[] BuildFrame(int round, string title, long occupied)
        {
            List<string> result = [];

            result.Add($"{title} // ROUND {round:000} // OCCUPIED {occupied:0000}");
            result.Add(string.Empty);

            for (long y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (long x = 0; x < this.Map.Width; x++)
                {
                    sb.Append((char)this.Map[y, x]);
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(new Frame(PadFrame(frame, width, height)));
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

        private void SetSeat(
            Dictionary<Vector<long>, List<VectorCell<long, Entity>>> visible,
            VectorCell<long, Entity> cell,
            VectorArray<long, Entity> map,
            ref bool stateChanged,
            Func<int, bool> equality,
            Entity value)
        {
            List<VectorCell<long, Entity>> adjacent = visible[cell.Point];

            int count = 0;

            foreach (var item in adjacent)
            {
                if (this.Map[item.Point] == Entity.OccupiedSeat)
                {
                    count++;
                }
            }

            if (equality(count))
            {
                map[cell.Point] = value;
                stateChanged = true;
            }
        }

        private Dictionary<Vector<long>, List<VectorCell<long, Entity>>> VisibleSeats()
        {
            Dictionary<Vector<long>, List<VectorCell<long, Entity>>> result = new();

            foreach (VectorCell<long, Entity> cell in this.Map.AxisEnumerator())
            {
                result.Add(cell.Point, new());

                foreach (VectorCell<long, Entity> cardinal in CardinalHelper.AllCells<long, Entity>())
                {
                    Vector<long> point = cell.Point.Clone();

                    while (true)
                    {
                        point += cardinal.Point;

                        if (this.Map.IsVectorInRange(point))
                        {
                            if (this.Map[point] != Entity.Floor)
                            {
                                result[cell.Point].Add(new(point, Entity.Floor, cardinal.Direction));
                                break;
                            }

                            continue;
                        }

                        break;
                    }
                }
            }

            return result;
        }
    }
}
