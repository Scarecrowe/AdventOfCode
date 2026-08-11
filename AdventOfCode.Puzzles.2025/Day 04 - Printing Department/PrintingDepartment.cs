namespace AdventOfCode.Puzzles._2025.Day_04___Printing_Department
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class PrintingDepartment
    {
        private const int MaxViewportWidth = 120;
        private const int MaxViewportHeight = 64;

        public PrintingDepartment(string[] input)
        {
            this.Map = new(input, (c) => c);
        }

        public PrintingDepartment(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<long, char> Map { get; set; }

        public long AccessableRolls()
        {
            long result = 0;

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value != '@')
                {
                    continue;
                }

                if (this.Map.AdjacentInterCardinal(cell.Point).Count(x => x.Value == '@') < 4)
                {
                    result++;
                }
            }

            return result;
        }

        public long AllAccessableRolls()
        {
            long result = 0;
            List<Vector<long>> rolls = [];

            while (true)
            {
                rolls.Clear();

                foreach (var cell in this.Map.AxisEnumerator())
                {
                    if (cell.Value != '@')
                    {
                        continue;
                    }

                    if (this.Map.AdjacentInterCardinal(cell.Point).Count(x => x.Value == '@') < 4)
                    {
                        rolls.Add(cell.Point);
                    }
                }

                if (rolls.Count == 0)
                {
                    break;
                }

                foreach (var point in rolls)
                {
                    this.Map[point] = '.';
                }

                result += rolls.Count;
            }

            return result;
        }

        public PrintingDepartment RenderSilver()
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<Vector<long>> accessible = this.FindAccessibleRolls();
            HashSet<Vector<long>> highlighted = [.. accessible];
            string title = $"PRINTING DEPARTMENT // ACCESSIBLE ROLLS {accessible.Count:0000}";

            List<string[]> frames = [];

            frames.Add(this.BuildFrame(title, highlighted, [], null));

            for (int i = 0; i < accessible.Count; i++)
            {
                if (i % 24 == 0 || i == accessible.Count - 1)
                {
                    HashSet<Vector<long>> revealed = [.. accessible.Take(i + 1)];
                    frames.Add(this.BuildFrame(
                        $"PRINTING DEPARTMENT // SCANNING {i + 1:0000}/{accessible.Count:0000}",
                        revealed,
                        [],
                        accessible[i]));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(title, highlighted, [], null));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public PrintingDepartment RenderGold(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            HashSet<Vector<long>> removed = [];
            long totalRemoved = 0;
            int wave = 0;

            frames.Add(this.BuildFrame("PRINTING DEPARTMENT // INITIAL STATE", [], removed, null));

            while (true)
            {
                List<Vector<long>> rolls = this.FindAccessibleRolls();

                if (rolls.Count == 0)
                {
                    break;
                }

                wave++;

                frames.Add(this.BuildFrame(
                    $"WAVE {wave:000} // ACCESSIBLE THIS WAVE {rolls.Count:0000} // TOTAL {totalRemoved:0000}",
                    [.. rolls],
                    removed,
                    this.CentreOf(rolls)));

                for (int i = 0; i < rolls.Count; i++)
                {
                    Vector<long> point = rolls[i];
                    this.Map[point] = '.';
                    removed.Add(point);
                    totalRemoved++;

                    if (i % renderEvery == 0 || i == rolls.Count - 1)
                    {
                        frames.Add(this.BuildFrame(
                            $"WAVE {wave:000} // REMOVING {i + 1:0000}/{rolls.Count:0000} // TOTAL {totalRemoved:0000}",
                            [point],
                            removed,
                            point));
                    }
                }
            }

            for (int i = 0; i < 36; i++)
            {
                frames.Add(this.BuildFrame(
                    $"STOP // TOTAL ROLLS REMOVED {totalRemoved:0000}",
                    [],
                    removed,
                    null));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<Vector<long>> FindAccessibleRolls()
        {
            List<Vector<long>> rolls = [];

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value != '@')
                {
                    continue;
                }

                if (this.Map.AdjacentInterCardinal(cell.Point).Count(x => x.Value == '@') < 4)
                {
                    rolls.Add(cell.Point);
                }
            }

            return rolls;
        }

        private string[] BuildFrame(
            string title,
            HashSet<Vector<long>> highlighted,
            HashSet<Vector<long>> removed,
            Vector<long>? focus)
        {
            List<string> result = [];
            Viewport viewport = this.GetViewport(focus ?? this.CentreOf(highlighted.Count > 0 ? [.. highlighted] : [.. removed]));

            result.Add(title);
            result.Add($"VIEW X {viewport.Left:000}-{viewport.Right - 1:000} / Y {viewport.Top:000}-{viewport.Bottom - 1:000}");
            result.Add(string.Empty);

            for (long y = viewport.Top; y < viewport.Bottom; y++)
            {
                StringBuilder sb = new();

                for (long x = viewport.Left; x < viewport.Right; x++)
                {
                    Vector<long> point = new(x, y);
                    char value = this.Map[point];

                    if (highlighted.Contains(point))
                    {
                        sb.Append('+');
                    }
                    else if (removed.Contains(point))
                    {
                        sb.Append('x');
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

        private Viewport GetViewport(Vector<long> focus)
        {
            long viewportWidth = Math.Min(this.Map.Width, MaxViewportWidth);
            long viewportHeight = Math.Min(this.Map.Height, MaxViewportHeight);

            long left = focus.X - viewportWidth / 2;
            long top = focus.Y - viewportHeight / 2;

            left = Math.Max(0, Math.Min(left, this.Map.Width - viewportWidth));
            top = Math.Max(0, Math.Min(top, this.Map.Height - viewportHeight));

            return new(left, top, left + viewportWidth, top + viewportHeight);
        }

        private Vector<long> CentreOf(IReadOnlyCollection<Vector<long>> points)
        {
            if (points.Count == 0)
            {
                return new(this.Map.Width / 2, this.Map.Height / 2);
            }

            return new(
                (long)Math.Round(points.Average(p => p.X)),
                (long)Math.Round(points.Average(p => p.Y)));
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

        private readonly record struct Viewport(long Left, long Top, long Right, long Bottom);
    }
}
