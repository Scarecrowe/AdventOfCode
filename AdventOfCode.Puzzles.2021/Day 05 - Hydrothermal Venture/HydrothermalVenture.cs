namespace AdventOfCode.Puzzles._2021.Day_05___Hydrothermal_Venture
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class HydrothermalVenture
    {
        public HydrothermalVenture(string[] input, bool includeDiagional)
        {
            this.Input = input;
            (List<HydrothermalVent> vents, Vector<int> point) = Parse(input);
            this.Map = new (point.X + 1, point.Y + 1);
            this.MapVents(vents, includeDiagional);
        }

        public HydrothermalVenture(string[] input, bool includeDiagional, IFrameRenderer renderer)
            : this(input, includeDiagional)
        {
            this.Renderer = renderer;
        }

        public string[] Input { get; }

        public IFrameRenderer? Renderer { get; }

        public VectorArray<int, int> Map { get; }

        public int TotalCrossOverVents() => this.Map.AxisEnumerator().Count(cell => cell.Value >= 2);

        public void MapHorizontally(HydrothermalVent vent)
        {
            for (int y = Math.Min(vent.Origin.Y, vent.Destination.Y); y <= Math.Max(vent.Origin.Y, vent.Destination.Y); y++)
            {
                this.Map[y, vent.Origin.X]++;
            }
        }

        public void MapVertically(HydrothermalVent vent)
        {
            for (int x = Math.Min(vent.Origin.X, vent.Destination.X); x <= Math.Max(vent.Origin.X, vent.Destination.X); x++)
            {
                this.Map[vent.Origin.Y, x]++;
            }
        }

        public void MapDiagionally(HydrothermalVent vent)
        {
            int x = vent.Origin.X;
            int y = vent.Origin.Y;

            this.Map[y, x]++;

            switch (vent.Direction)
            {
                case Cardinal.NorthEast:
                    this.MoveNorthEast(x, y, vent);
                    return;
                case Cardinal.SouthEast:
                    this.MoveSouthEast(x, y, vent);
                    return;

                case Cardinal.SouthWest:
                    this.MoveSouthWest(x, y, vent);
                    return;

                case Cardinal.NorthWest:
                    this.MoveNorthWest(x, y, vent);
                    return;
            }
        }

        public HydrothermalVenture RenderSilver(int renderEvery = 1000)
            => this.Render(false, renderEvery);

        public HydrothermalVenture RenderGold(int renderEvery = 2000)
            => this.Render(true, renderEvery);

        private HydrothermalVenture Render(bool includeDiagional, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            (List<HydrothermalVent> vents, Vector<int> max) = Parse(this.Input);

            VectorArray<int, int> map = new(max.X + 1, max.Y + 1);

            int plotted = 0;
            int danger = 0;

            int scale = Math.Max(1, Math.Max(map.Width, map.Height) / 120);

            foreach (HydrothermalVent vent in vents)
            {
                if (!vent.IsHorizontal &&
                    !vent.IsVertical &&
                    (!includeDiagional || !vent.IsDiagional))
                {
                    continue;
                }

                foreach (Vector<int> point in PointsOnLine(vent))
                {
                    map[point.Y, point.X]++;

                    if (map[point.Y, point.X] == 2)
                    {
                        danger++;
                    }

                    plotted++;

                    if (plotted % renderEvery == 0)
                    {
                        this.Renderer.RenderFrame(
                            new Frame(
                                this.BuildFrame(
                                    map,
                                    point,
                                    scale,
                                    includeDiagional
                                        ? $"HYDROTHERMAL VENTURE // ALL LINES // DANGER {danger:00000}"
                                        : $"HYDROTHERMAL VENTURE // STRAIGHT LINES // DANGER {danger:00000}")));
                    }
                }
            }

            return this;
        }

        private static (List<HydrothermalVent> Vents, Vector<int> Point) Parse(string[] input)
        {
            List<HydrothermalVent> result = new();
            Vector<int> point = new(0, 0);

            foreach (string line in input)
            {
                string[] tokens = line.Split(" -> ");
                HydrothermalVent vent = new(new(tokens[0].Split(",").ToInt()), new(tokens[1].Split(",").ToInt()));

                point.X = Math.Max(point.X, vent.Origin.X);
                point.X = Math.Max(point.X, vent.Destination.X);
                point.Y = Math.Max(point.Y, vent.Origin.Y);
                point.Y = Math.Max(point.Y, vent.Destination.Y);

                result.Add(vent);
            }

            return (result, point);
        }

        private void MapVents(List<HydrothermalVent> vents, bool includeDiagional)
        {
            foreach (HydrothermalVent vent in vents)
            {
                if (vent.IsHorizontal)
                {
                    this.MapHorizontally(vent);
                    continue;
                }
                else if (vent.IsVertical)
                {
                    this.MapVertically(vent);
                    continue;
                }
                else if (!includeDiagional)
                {
                    continue;
                }

                if (vent.IsDiagional)
                {
                    this.MapDiagionally(vent);
                }
            }
        }

        private void MoveNorthEast(int x, int y, HydrothermalVent vent)
        {
            for (int i = 1; i <= Math.Abs(vent.Origin.X - vent.Destination.X); i++)
            {
                x++;
                y--;
                this.Map[y, x]++;
            }
        }

        private void MoveSouthEast(int x, int y, HydrothermalVent vent)
        {
            for (int i = 1; i <= Math.Abs(vent.Origin.X - vent.Destination.X); i++)
            {
                x++;
                y++;
                this.Map[y, x]++;
            }
        }

        private void MoveSouthWest(int x, int y, HydrothermalVent vent)
        {
            for (int i = 1; i <= Math.Abs(vent.Origin.X - vent.Destination.X); i++)
            {
                x--;
                y++;
                this.Map[y, x]++;
            }
        }

        private void MoveNorthWest(int x, int y, HydrothermalVent vent)
        {
            for (int i = 1; i <= Math.Abs(vent.Origin.X - vent.Destination.X); i++)
            {
                x--;
                y--;
                this.Map[y, x]++;
            }
        }

        private static IEnumerable<Vector<int>> PointsOnLine(HydrothermalVent vent)
        {
            int x = vent.Origin.X;
            int y = vent.Origin.Y;

            int dx = Math.Sign(vent.Destination.X - vent.Origin.X);
            int dy = Math.Sign(vent.Destination.Y - vent.Origin.Y);

            yield return new(x, y);

            while (x != vent.Destination.X || y != vent.Destination.Y)
            {
                x += dx;
                y += dy;

                yield return new(x, y);
            }
        }

        private static int CountDanger(VectorArray<int, int> map)
            => map.AxisEnumerator().Count(cell => cell.Value >= 2);

        private string[] BuildFrame(
            VectorArray<int, int> map,
            Vector<int> current,
            int scale,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < map.Height; y += scale)
            {
                StringBuilder sb = new();

                for (int x = 0; x < map.Width; x += scale)
                {
                    if (current.X >= x &&
                        current.X < x + scale &&
                        current.Y >= y &&
                        current.Y < y + scale)
                    {
                        sb.Append('@');
                        continue;
                    }

                    int max = 0;

                    for (int yy = y; yy < Math.Min(y + scale, map.Height); yy++)
                    {
                        for (int xx = x; xx < Math.Min(x + scale, map.Width); xx++)
                        {
                            max = Math.Max(max, map[yy, xx]);
                        }
                    }

                    sb.Append(max switch
                    {
                        0 => '.',
                        >= 10 => '+',
                        _ => (char)('0' + Math.Min(max, 9))
                    });
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
    }
}
