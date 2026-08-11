namespace AdventOfCode.Puzzles._2019.Day_19___Tractor_Beam
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;
    using System.Text;

    public class TractorBeam
    {
        public TractorBeam(string program)
        {
            this.Cpu = new(program);
            this.Map = new();
        }

        public TractorBeam(string program, IFrameRenderer renderer)
            : this(program)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public IntcodeCpu Cpu { get; }

        public int TractorBeamArea => this.Map.Count(c => c.Value == Entity.Pulled);

        private VectorDictionary<int, Entity> Map { get; }

        public TractorBeam BuildMap(int size, int min, int max)
        {
            for (int y = min; y < max; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    this.Cpu.Reset();
                    this.Cpu.Input.Enqueue(x);
                    this.Cpu.Input.Enqueue(y);
                    this.Cpu.Run();
                    this.Map.Add(new(x, y), (Entity)this.Cpu.Output.Dequeue());
                }
            }

            return this;
        }

        public TractorBeam PrintMap()
        {
            long minY = this.Map.Min(c => c.Key.Y);
            long maxY = this.Map.Max(c => c.Key.Y);
            long minX = this.Map.Min(c => c.Key.X);
            long maxX = this.Map.Max(c => c.Key.X);

            Dictionary<Entity, string> display = new()
            {
                { Entity.Stationary, "." },
                { Entity.Pulled, "#" }
            };

            for (long y = minY; y <= maxY; y++)
            {
                for (long x = minX; x <= maxX; x++)
                {
                    if (this.Map.ContainsKey(new(x, y)))
                    {
                        PuzzleConsole.Write(display[this.Map[new(x, y)]]);
                    }
                    else
                    {
                        PuzzleConsole.Write("#");
                    }
                }

                PuzzleConsole.WriteLine();
            }

            return this;
        }

        public TractorBeam RenderSilver(int size = 50, int renderEvery = 25)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            int step = 0;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    this.Probe(x, y);
                    step++;

                    if (step % renderEvery == 0)
                    {
                        frames.Add(this.BuildFrame(0, 0, size, size, null, $"TRACTOR BEAM SCAN // AREA {this.TractorBeamArea:000}"));
                    }
                }
            }

            frames.Add(this.BuildFrame(0, 0, size, size, null, $"SCAN COMPLETE // AREA {this.TractorBeamArea:000}"));

            this.RenderPaddedFrames(frames);

            return this;
        }

        public TractorBeam RenderGold(
    int shipSize = 100,
    int viewportWidth = 220,
    int viewportHeight = 70,
    int renderEvery = 10)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];

            int x = 0;

            for (int y = shipSize; ; y++)
            {
                while (this.Probe(x, y) != Entity.Pulled)
                {
                    x++;
                }

                Vector<int> candidate = new(x, y - shipSize + 1);

                if (y % renderEvery == 0)
                {
                    frames.Add(this.BuildGoldFrame(
                        candidate,
                        viewportWidth,
                        viewportHeight,
                        shipSize,
                        $"SEARCHING FOR {shipSize}x{shipSize} SHIP // X {candidate.X:0000} Y {candidate.Y:0000}"));
                }

                if (this.Probe(x + shipSize - 1, y - shipSize + 1) == Entity.Pulled)
                {
                    long answer = (candidate.X * 10000L) + candidate.Y;

                    frames.Add(this.BuildGoldFrame(
                            candidate,
                            viewportWidth,
                            viewportHeight,
                            shipSize,
                            $"SHIP LOCKED // ANSWER {answer}"));

                    this.RenderPaddedFrames(frames);

                    return this;
                }
            }
        }

        public long ClosestPoint()
        {
            long minY = this.Map.Min(c => c.Key.Y);
            long maxY = this.Map.Max(c => c.Key.Y);
            long minX = this.Map.Min(c => c.Key.X);
            long maxX = this.Map.Max(c => c.Key.X);

            int highest = 0;

            for (long y = minY; y <= maxY; y++)
            {
                int count = this.Map.Count(c => c.Key.Y == y && c.Value == Entity.Pulled);

                if (count >= 100)
                {
                    long xFirst = this.Map.FirstOrDefault(c => c.Key.Y == y && c.Value == Entity.Pulled).Key.X;
                    long xLast = this.Map.LastOrDefault(c => c.Key.Y == y && c.Value == Entity.Pulled).Key.X;

                    for (long x = xFirst; x <= xLast; x++)
                    {
                        count = 0;

                        for (long yy = y; yy <= y + 100; yy++)
                        {
                            if (this.Map.ContainsKey(new(x, yy)))
                            {
                                count += (int)this.Map[new(x, yy)];
                            }
                        }

                        if (count > highest)
                        {
                            highest = count;
                        }

                        if (count == 100)
                        {
                            if (this.Map[new(x + 99, y)] == Entity.Pulled && this.Map[new(x, y + 99)] == Entity.Pulled)
                            {
                                return (x * 10000) + y;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            if (this.Renderer == null || frames.Count == 0)
            {
                return;
            }

            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer.RenderFrame(new Frame(PadFrame(frame, width, height)));
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

        private Entity Probe(int x, int y)
        {
            Vector<int> point = new(x, y);

            if (this.Map.TryGetValue(point, out Entity cached))
            {
                return cached;
            }

            this.Cpu.Reset();
            this.Cpu.Input.Enqueue(x);
            this.Cpu.Input.Enqueue(y);
            this.Cpu.Run();

            Entity result = (Entity)this.Cpu.Output.Dequeue();

            this.Map.Add(point, result);

            return result;
        }

        private string[] BuildFrame(
            int startX,
            int startY,
            int width,
            int height,
            Vector<int>? ship,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = startY; y < startY + height; y++)
            {
                StringBuilder sb = new();

                for (int x = startX; x < startX + width; x++)
                {
                    Vector<int> point = new(x, y);

                    if (ship != null && IsShipOutline(x, y, ship, 100))
                    {
                        sb.Append('*');
                    }
                    else if (x < 0 || y < 0)
                    {
                        sb.Append(' ');
                    }
                    else if (this.Map.TryGetValue(point, out Entity entity))
                    {
                        sb.Append(entity == Entity.Pulled ? '#' : '.');
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private string[] BuildGoldFrame(
            Vector<int> ship,
            int viewportWidth,
            int viewportHeight,
            int shipSize,
            string title)
        {
            int startX = ship.X - ((viewportWidth - shipSize) / 2);
            int startY = ship.Y - ((viewportHeight - shipSize) / 2);

            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = startY; y < startY + viewportHeight; y++)
            {
                StringBuilder sb = new();

                for (int x = startX; x < startX + viewportWidth; x++)
                {
                    if (x < 0 || y < 0)
                    {
                        sb.Append(' ');
                        continue;
                    }

                    if (IsShipOutline(x, y, ship, shipSize))
                    {
                        sb.Append('*');
                    }
                    else
                    {
                        sb.Append(this.Probe(x, y) == Entity.Pulled ? '#' : '.');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static bool IsShipOutline(int x, int y, Vector<int> ship, int shipSize)
        {
            bool inside =
                x >= ship.X &&
                x < ship.X + shipSize &&
                y >= ship.Y &&
                y < ship.Y + shipSize;

            if (!inside)
            {
                return false;
            }

            return x == ship.X ||
                   x == ship.X + shipSize - 1 ||
                   y == ship.Y ||
                   y == ship.Y + shipSize - 1;
        }
    }
}
