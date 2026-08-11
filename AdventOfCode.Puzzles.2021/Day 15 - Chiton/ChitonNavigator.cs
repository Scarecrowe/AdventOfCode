namespace AdventOfCode.Puzzles._2021.Day_15___Chiton
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class ChitonNavigator
    {
        public ChitonNavigator(string[] input)
            => this.Map = Parse(input);

        public ChitonNavigator(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public VectorArray<int, Chiton> Map { get; private set; }

        public ChitonNavigator Enlarge()
        {
            int originalWidth = this.Map.Width;
            int originalHeight = this.Map.Height;

            VectorArray<int, Chiton> enlargedMap = new(originalWidth * 5, originalHeight * 5);

            for (int tileY = 0; tileY < 5; tileY++)
            {
                for (int tileX = 0; tileX < 5; tileX++)
                {
                    foreach (VectorCell<int, Chiton> cell in this.Map.AxisEnumerator())
                    {
                        Vector<int> point = new(
                            cell.Point.X + originalWidth * tileX,
                            cell.Point.Y + originalHeight * tileY);

                        int risk = ((cell.Value.Risk + tileX + tileY - 1) % 9) + 1;

                        enlargedMap[point] = new(new(point), risk);
                    }
                }
            }

            this.Map = enlargedMap;

            return this;
        }

        public int Navigate()
            => this.NavigateCore(
                renderEvery: null,
                searchViewportWidth: null,
                searchViewportHeight: null,
                finalViewportWidth: null,
                finalViewportHeight: null,
                quietSearch: false,
                smoothFinalCamera: false);

        public ChitonNavigator RenderSilver(int renderEvery = 75)
        {
            this.NavigateCore(
                renderEvery,
                searchViewportWidth: null,
                searchViewportHeight: null,
                finalViewportWidth: 60,
                finalViewportHeight: this.Map.Height,
                quietSearch: false,
                smoothFinalCamera: false);

            return this;
        }

        public ChitonNavigator RenderGold(int renderEvery = 2000)
        {
            this.NavigateCore(
                renderEvery,
                searchViewportWidth: 120,
                searchViewportHeight: 60,
                finalViewportWidth: 120,
                finalViewportHeight: 60,
                quietSearch: true,
                smoothFinalCamera: true);

            return this;
        }

        private int NavigateCore(
            int? renderEvery,
            int? searchViewportWidth,
            int? searchViewportHeight,
            int? finalViewportWidth,
            int? finalViewportHeight,
            bool quietSearch,
            bool smoothFinalCamera)
        {
            PriorityQueue<Chiton, int> queue = new();

            Dictionary<Vector<int>, Vector<int>> cameFrom = [];
            HashSet<Vector<int>> frontier = [];
            HashSet<Vector<int>> settled = [];

            Chiton start = this.Map[0, 0];
            start.TotalRisk = 0;

            queue.Enqueue(start, 0);
            frontier.Add(start.Point);

            Vector<int> target = new(this.Map.Width - 1, this.Map.Height - 1);

            int frame = 0;

            while (queue.Count > 0)
            {
                Chiton chiton = queue.Dequeue();
                frontier.Remove(chiton.Point);

                if (chiton.Visited)
                {
                    continue;
                }

                chiton.Visited = true;
                settled.Add(chiton.Point);

                if (renderEvery.HasValue && frame++ % renderEvery.Value == 0)
                {
                    this.Renderer?.RenderFrame(new Frame(this.BuildFrameClamped(
                        current: chiton.Point,
                        settled,
                        frontier,
                        path: [],
                        title: $"CHITON // SEARCHING // RISK {chiton.TotalRisk:00000}",
                        searchViewportWidth,
                        searchViewportHeight,
                        quietSearch)));
                }

                if (chiton.Point == target)
                {
                    List<Vector<int>> path = ReconstructPath(cameFrom, target);
                    HashSet<Vector<int>> pathSet = [];

                    double cameraX = path[0].X;
                    double cameraY = path[0].Y;

                    foreach (Vector<int> point in path)
                    {
                        pathSet.Add(point);

                        Vector<int> cameraCentre;

                        if (smoothFinalCamera)
                        {
                            cameraX += (point.X - cameraX) * 0.18;
                            cameraY += (point.Y - cameraY) * 0.18;

                            cameraCentre = new(
                                (int)Math.Round(cameraX),
                                (int)Math.Round(cameraY));
                        }
                        else
                        {
                            cameraCentre = point;
                        }

                        this.Renderer?.RenderFrame(new Frame(this.BuildFrameCentred(
                            current: point,
                            cameraCentre,
                            path: pathSet,
                            title: $"CHITON // FINAL PATH // RISK {chiton.TotalRisk:00000}",
                            finalViewportWidth,
                            finalViewportHeight,
                            quiet: false)));
                    }

                    return chiton.TotalRisk;
                }

                foreach (VectorCell<int, Chiton> adjacent in this.Map.AdjacentCardinal(chiton.Point))
                {
                    if (adjacent.Value.Visited)
                    {
                        continue;
                    }

                    int risk = chiton.TotalRisk + adjacent.Value.Risk;

                    if (risk < adjacent.Value.TotalRisk)
                    {
                        adjacent.Value.TotalRisk = risk;
                        cameFrom[adjacent.Point] = chiton.Point;

                        queue.Enqueue(adjacent.Value, adjacent.Value.TotalRisk);
                        frontier.Add(adjacent.Point);
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private string[] BuildFrameCentred(
            Vector<int> current,
            Vector<int> cameraCentre,
            HashSet<Vector<int>> path,
            string title,
            int? viewportWidth,
            int? viewportHeight,
            bool quiet)
        {
            int width = viewportWidth ?? this.Map.Width;
            int height = viewportHeight ?? this.Map.Height;

            int startX = cameraCentre.X - width / 2;
            int startY = cameraCentre.Y - height / 2;

            return this.BuildFrameFixedWidth(
                current,
                settled: [],
                frontier: [],
                path,
                title,
                startX,
                startY,
                width,
                height,
                showCamera: viewportWidth.HasValue || viewportHeight.HasValue,
                quiet);
        }

        private string[] BuildFrameClamped(
            Vector<int> current,
            HashSet<Vector<int>> settled,
            HashSet<Vector<int>> frontier,
            HashSet<Vector<int>> path,
            string title,
            int? viewportWidth,
            int? viewportHeight,
            bool quiet)
        {
            int width = Math.Min(viewportWidth ?? this.Map.Width, this.Map.Width);
            int height = Math.Min(viewportHeight ?? this.Map.Height, this.Map.Height);

            int startX = Math.Clamp(current.X - width / 2, 0, this.Map.Width - width);
            int startY = Math.Clamp(current.Y - height / 2, 0, this.Map.Height - height);

            return this.BuildFrameFixedWidth(
                current,
                settled,
                frontier,
                path,
                title,
                startX,
                startY,
                width,
                height,
                showCamera: viewportWidth.HasValue || viewportHeight.HasValue,
                quiet);
        }

        private string[] BuildFrameFixedWidth(
            Vector<int> current,
            HashSet<Vector<int>> settled,
            HashSet<Vector<int>> frontier,
            HashSet<Vector<int>> path,
            string title,
            int startX,
            int startY,
            int width,
            int height,
            bool showCamera,
            bool quiet)
        {
            List<string> result = [];

            result.Add(FitLine(title, width));

            if (showCamera)
            {
                result.Add(FitLine(
                    $"CAMERA X {startX}-{startX + width - 1} // Y {startY}-{startY + height - 1}",
                    width));
            }

            result.Add(new string(' ', width));

            Vector<int> target = new(this.Map.Width - 1, this.Map.Height - 1);

            for (int screenY = 0; screenY < height; screenY++)
            {
                StringBuilder sb = new();

                int mapY = startY + screenY;

                for (int screenX = 0; screenX < width; screenX++)
                {
                    int mapX = startX + screenX;

                    if (mapX < 0 || mapY < 0 || mapX >= this.Map.Width || mapY >= this.Map.Height)
                    {
                        sb.Append(' ');
                        continue;
                    }

                    Vector<int> point = new(mapX, mapY);

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (point == target)
                    {
                        sb.Append('X');
                    }
                    else if (path.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else if (frontier.Contains(point))
                    {
                        sb.Append('*');
                    }
                    else if (settled.Contains(point))
                    {
                        sb.Append('.');
                    }
                    else
                    {
                        sb.Append(quiet ? ' ' : this.Map[mapY, mapX].Risk);
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static string FitLine(string value, int width)
        {
            if (value.Length > width)
            {
                return value[..width];
            }

            return value.PadRight(width, ' ');
        }

        private static List<Vector<int>> ReconstructPath(
            Dictionary<Vector<int>, Vector<int>> cameFrom,
            Vector<int> target)
        {
            List<Vector<int>> path = [];
            Vector<int> current = target;

            path.Add(current);

            while (cameFrom.TryGetValue(current, out Vector<int> previous))
            {
                current = previous;
                path.Add(current);
            }

            path.Reverse();

            return path;
        }

        private static VectorArray<int, Chiton> Parse(string[] input)
        {
            VectorArray<int, Chiton> result = new(input[0].Length, input.Length);

            foreach (VectorCell<int, Chiton> cell in result.AxisEnumerator())
            {
                result[cell.Point] = new(
                    new(cell.Point),
                    input[cell.Point.Y][cell.Point.X].ToString().ToInt());
            }

            return result;
        }
    }
}