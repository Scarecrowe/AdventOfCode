namespace AdventOfCode.Puzzles._2021.Day_17___Trick_Shot
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class TrickShot
    {
        private const int ViewWidth = 140;
        private const int ViewHeight = 46;
        private const int CanvasWidth = ViewWidth * 4;
        private const int BallX = (ViewWidth / 2) - 8;

        public TrickShot(string[] input)
        {
            this.Parse(input);
        }

        public TrickShot(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public Vector<int> Min { get; private set; }

        public Vector<int> Max { get; private set; }

        private sealed record Shot(Vector<int> Velocity, List<Vector<int>> Path, int Highest);

        public long Simulate(bool highest)
        {
            List<Shot> hits = this.FindHits();

            return highest
                ? hits.Max(x => x.Highest)
                : hits.Count;
        }

        public TrickShot RenderSilver(int renderEvery = 2)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            Shot best = this.FindHits()
                .OrderByDescending(x => x.Highest)
                .First();

            List<Vector<int>> displayPath = this.BuildCompressedPath(best.Path);

            for (int i = 0; i < displayPath.Count; i++)
            {
                if (i % renderEvery == 0 || i == displayPath.Count - 1)
                {
                    this.Renderer.RenderFrame(new Frame(this.BuildSilverFrame(
                        displayPath,
                        i,
                        best,
                        $"BEST TRICK SHOT // VELOCITY {best.Velocity.X},{best.Velocity.Y} // PEAK {best.Highest}")));
                }
            }

            this.Renderer.RenderFrame(new Frame(this.BuildSilverFrame(
                    displayPath,
                    displayPath.Count - 1,
                    best,
                    $"TARGET HIT // VELOCITY {best.Velocity.X},{best.Velocity.Y} // PEAK {best.Highest}")));

            return this;
        }

        private string[] BuildSilverFrame(
            List<Vector<int>> displayPath,
            int visibleIndex,
            Shot shot,
            string title)
        {
            char[,] grid = CreateGrid(ViewHeight, ViewWidth, '.');

            Vector<int> focus = displayPath[visibleIndex];

            for (int i = 0; i <= visibleIndex; i++)
            {
                Vector<int> point = displayPath[i];

                int x = point.X - focus.X + BallX;

                if (x >= 0 && x < ViewWidth && point.Y >= 0 && point.Y < ViewHeight)
                {
                    grid[point.Y, x] = '#';
                }
            }

            Vector<int> start = displayPath[0];
            int startX = BallX + start.X - focus.X;

            if (startX >= 0 && startX < ViewWidth && start.Y >= 0 && start.Y < ViewHeight)
            {
                grid[start.Y, startX] = 'S';
            }

            grid[focus.Y, BallX] = '@';

            List<string> result = [];

            result.Add($"{title} // STEP {visibleIndex + 1:0000}/{displayPath.Count:0000}");
            result.Add(string.Empty);

            AddGridRows(result, grid);

            return [.. result];
        }

        private List<Vector<int>> BuildCompressedPath(List<Vector<int>> path)
        {
            List<Vector<int>> result = [];

            int minY = path.Min(p => p.Y);
            int maxY = path.Max(p => p.Y);

            for (int i = 0; i < path.Count; i++)
            {
                Vector<int> point = path[i];

                int x = Scale(i, 0, path.Count - 1, 0, CanvasWidth - 1);
                int y = Scale(point.Y, minY, maxY, ViewHeight - 1, 0);

                Vector<int> display = new(x, y);

                if (result.Count == 0 || result.Last() != display)
                {
                    result.Add(display);
                }
            }

            return result;
        }

        private List<Vector<int>> BuildVelocityCells(List<Shot> hits)
        {
            int minX = hits.Min(x => x.Velocity.X);
            int maxX = hits.Max(x => x.Velocity.X);
            int minY = hits.Min(x => x.Velocity.Y);
            int maxY = hits.Max(x => x.Velocity.Y);

            HashSet<Vector<int>> cells = [];

            foreach (Shot hit in hits)
            {
                int x = Scale(hit.Velocity.X, minX, maxX, 0, CanvasWidth - 1);
                int y = Scale(hit.Velocity.Y, minY, maxY, ViewHeight - 1, 0);

                cells.Add(new(x, y));
            }

            return cells
                .OrderBy(c => c.Y)
                .ThenBy(c => c.X)
                .ToList();
        }

        private List<Shot> FindHits()
        {
            List<Shot> hits = [];

            int minVy = this.Min.Y;
            int maxVy = Math.Abs(this.Min.Y) - 1;

            for (int x = 0; x <= this.Max.X; x++)
            {
                for (int y = minVy; y <= maxVy; y++)
                {
                    Shot? shot = this.Fire(new(x, y));

                    if (shot != null)
                    {
                        hits.Add(shot);
                    }
                }
            }

            return hits;
        }

        private Shot? Fire(Vector<int> velocity)
        {
            Vector<int> current = new(0, 0);
            Vector<int> step = new(velocity);
            List<Vector<int>> path = [];
            int highest = 0;

            while (current.X <= this.Max.X && current.Y >= this.Min.Y)
            {
                current.X += step.X;
                current.Y += step.Y;

                path.Add(new(current));
                highest = Math.Max(highest, current.Y);

                if (this.IsTarget(current))
                {
                    return new(velocity, path, highest);
                }

                if (step.X > 0)
                {
                    step.X--;
                }
                else if (step.X < 0)
                {
                    step.X++;
                }

                step.Y--;
            }

            return null;
        }

        private bool IsTarget(Vector<int> point)
            => point.X >= this.Min.X
            && point.X <= this.Max.X
            && point.Y >= this.Min.Y
            && point.Y <= this.Max.Y;

        private static char[,] CreateGrid(int height, int width, char fill)
        {
            char[,] grid = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = fill;
                }
            }

            return grid;
        }

        private static void AddGridRows(List<string> result, char[,] grid)
        {
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < width; x++)
                {
                    sb.Append(grid[y, x]);
                }

                result.Add(sb.ToString());
            }
        }

        private static int Scale(
            int value,
            int sourceMin,
            int sourceMax,
            int targetMin,
            int targetMax)
        {
            if (sourceMax == sourceMin)
            {
                return targetMin;
            }

            double progress = (value - sourceMin) / (double)(sourceMax - sourceMin);
            int result = targetMin + (int)Math.Round(progress * (targetMax - targetMin));

            return Math.Clamp(
                result,
                Math.Min(targetMin, targetMax),
                Math.Max(targetMin, targetMax));
        }

        private void Parse(string[] input)
        {
            string[] tokens = input[0]
                .Replace("target area: ", string.Empty)
                .Split(", ");

            int[] tokensX = tokens[0]
                .Replace("x=", string.Empty)
                .Split("..")
                .ToInt();

            int[] tokensY = tokens[1]
                .Replace("y=", string.Empty)
                .Split("..")
                .ToInt();

            this.Min = new(tokensX.Min(), tokensY.Min());
            this.Max = new(tokensX.Max(), tokensY.Max());
        }
    }
}