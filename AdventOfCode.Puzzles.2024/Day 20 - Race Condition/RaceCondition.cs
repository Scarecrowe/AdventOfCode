namespace AdventOfCode.Puzzles._2024.Day_20___Race_Condition
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

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

        public RaceCondition(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct Cheat(Vector<int> From, Vector<int> To, int Cost, int Saved);

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

        public RaceCondition RenderSilver(
            int renderEvery = 3,
            int viewportWidth = 96,
            int viewportHeight = 52,
            int minimumSaving = 100,
            int cheatsToShow = 24)
        {
            return this.RenderRace(
                title: "RACE CONDITION // 2PS CHEAT",
                range: 2,
                renderEvery: renderEvery,
                viewportWidth: viewportWidth,
                viewportHeight: viewportHeight,
                minimumSaving: minimumSaving,
                cheatsToShow: cheatsToShow);
        }

        public RaceCondition RenderGold(
            int renderEvery = 4,
            int viewportWidth = 96,
            int viewportHeight = 52,
            int minimumSaving = 100,
            int cheatsToShow = 32)
        {
            return this.RenderRace(
                title: "RACE CONDITION // 20PS CHEAT",
                range: 20,
                renderEvery: renderEvery,
                viewportWidth: viewportWidth,
                viewportHeight: viewportHeight,
                minimumSaving: minimumSaving,
                cheatsToShow: cheatsToShow);
        }

        private RaceCondition RenderRace(
            string title,
            int range,
            int renderEvery,
            int viewportWidth,
            int viewportHeight,
            int minimumSaving,
            int cheatsToShow)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            viewportWidth = Math.Min(viewportWidth, this.Map.Width);
            viewportHeight = Math.Min(viewportHeight, this.Map.Height);

            List<Vector<int>> path = this.GetNormalPath();
            HashSet<Vector<int>> trail = [];
            List<string[]> frames = [];

            for (int i = 0; i < path.Count; i++)
            {
                Vector<int> point = path[i];
                trail.Add(point);

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        title: $"{title} // NORMAL PATH // STEP {i:00000}/{path.Count - 1:00000}",
                        focus: point,
                        viewportWidth: viewportWidth,
                        viewportHeight: viewportHeight,
                        trail: trail));
                }
            }

            List<Cheat> cheats = this.GetCheats(range, minimumSaving)
                .OrderByDescending(c => c.Saved)
                .ThenBy(c => c.Cost)
                .ThenBy(c => c.From.Y)
                .ThenBy(c => c.From.X)
                .Take(cheatsToShow)
                .ToList();

            for (int i = 0; i < cheats.Count; i++)
            {
                Cheat cheat = cheats[i];
                List<Vector<int>> cheatPath = this.BuildCheatPath(cheat.From, cheat.To);
                Vector<int> focus = cheatPath.Count == 0 ? cheat.From : cheatPath[cheatPath.Count / 2];

                for (int hold = 0; hold < 4; hold++)
                {
                    frames.Add(this.BuildFrame(
                        title: $"{title} // CHEAT {i + 1:000}/{cheats.Count:000} // SAVES {cheat.Saved:0000} // COST {cheat.Cost:00}PS",
                        focus: focus,
                        viewportWidth: viewportWidth,
                        viewportHeight: viewportHeight,
                        trail: trail,
                        cheat: cheat,
                        cheatPath: cheatPath));
                }
            }

            int total = this.CountCheats(range);

            Vector<int> finalFocus = path.Count == 0 ? this.End : path.Last();

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    title: $"{title} // {total:0000000} CHEATS SAVE AT LEAST {minimumSaving}PS",
                    focus: finalFocus,
                    viewportWidth: viewportWidth,
                    viewportHeight: viewportHeight,
                    trail: trail));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<Vector<int>> GetNormalPath()
        {
            List<Vector<int>> path = [this.Start];
            HashSet<Vector<int>> seen = [this.Start];
            Vector<int> current = this.Start;

            while (current != this.End)
            {
                if (!this.Distance.TryGetValue(current, out int currentDistance))
                {
                    break;
                }

                Vector<int>? next = null;

                foreach (var adjacent in this.Map.AdjacentCardinal(current))
                {
                    if (adjacent.Value != '.')
                    {
                        continue;
                    }

                    if (!this.Distance.TryGetValue(adjacent.Point, out int nextDistance))
                    {
                        continue;
                    }

                    if (nextDistance == currentDistance - 1 && !seen.Contains(adjacent.Point))
                    {
                        next = adjacent.Point;
                        break;
                    }
                }

                if (next == null)
                {
                    break;
                }

                current = next;
                seen.Add(current);
                path.Add(current);
            }

            return path;
        }

        private List<Cheat> GetCheats(int range, int minimumSaving)
        {
            List<Cheat> cheats = [];

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value != '.')
                {
                    continue;
                }

                Vector<int> from = cell.Point;

                if (!this.Distance.TryGetValue(from, out int distFromEnd))
                {
                    continue;
                }

                for (int dy = -range; dy <= range; dy++)
                {
                    for (int dx = -range; dx <= range; dx++)
                    {
                        int cost = Math.Abs(dx) + Math.Abs(dy);

                        if (cost == 0 || cost > range)
                        {
                            continue;
                        }

                        Vector<int> to = new(from.X + dx, from.Y + dy);

                        if (!this.Map.IsVectorInRange(to) || this.Map[to] != '.')
                        {
                            continue;
                        }

                        if (!this.Distance.TryGetValue(to, out int distToEnd))
                        {
                            continue;
                        }

                        int saved = distFromEnd - distToEnd - cost;

                        if (saved >= minimumSaving)
                        {
                            cheats.Add(new(from, to, cost, saved));
                        }
                    }
                }
            }

            return cheats;
        }

        private List<Vector<int>> BuildCheatPath(Vector<int> from, Vector<int> to)
        {
            List<Vector<int>> path = [];
            Vector<int> current = from;

            int dx = Math.Sign(to.X - from.X);
            while (current.X != to.X)
            {
                current = new(current.X + dx, current.Y);
                path.Add(current);
            }

            int dy = Math.Sign(to.Y - from.Y);
            while (current.Y != to.Y)
            {
                current = new(current.X, current.Y + dy);
                path.Add(current);
            }

            return path;
        }

        private string[] BuildFrame(
            string title,
            Vector<int> focus,
            int viewportWidth,
            int viewportHeight,
            HashSet<Vector<int>> trail,
            Cheat? cheat = null,
            List<Vector<int>>? cheatPath = null)
        {
            List<string> result = [];
            HashSet<Vector<int>> cheatCells = cheatPath?.ToHashSet() ?? [];

            int left = Math.Clamp(focus.X - viewportWidth / 2, 0, Math.Max(0, this.Map.Width - viewportWidth));
            int top = Math.Clamp(focus.Y - viewportHeight / 2, 0, Math.Max(0, this.Map.Height - viewportHeight));

            result.Add(title);
            result.Add($"VIEW X {left:000}-{left + viewportWidth - 1:000} Y {top:000}-{top + viewportHeight - 1:000}");
            result.Add(string.Empty);

            for (int y = top; y < top + viewportHeight; y++)
            {
                StringBuilder sb = new();

                for (int x = left; x < left + viewportWidth; x++)
                {
                    Vector<int> point = new(x, y);
                    char value = this.Map[point];

                    if (point == this.Start)
                    {
                        sb.Append('S');
                    }
                    else if (point == this.End)
                    {
                        sb.Append('E');
                    }
                    else if (cheat != null && point == cheat.Value.From)
                    {
                        sb.Append('C');
                    }
                    else if (cheat != null && point == cheat.Value.To)
                    {
                        sb.Append('T');
                    }
                    else if (cheatCells.Contains(point))
                    {
                        sb.Append('x');
                    }
                    else if (point == focus)
                    {
                        sb.Append('@');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append('~');
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
