namespace AdventOfCode.Puzzles._2020.Day_03___Toboggan_Trajectory
{
    using AdventOfCode.Animation.Renderers;
    using System.Text;

    public class TobogganTrajectory
    {
        private const int ViewHeight = 20;
        private const int ViewTopPadding = 6;

        public TobogganTrajectory(string[] input) => this.Input = input;

        public TobogganTrajectory(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public string[] Input { get; }

        public IFrameRenderer? Renderer { get; }

        public int Single() => this.TraverseSlope(3, 1);

        public long Multiple()
        {
            long trees = this.TraverseSlope(1, 1);
            trees *= this.TraverseSlope(3, 1);
            trees *= this.TraverseSlope(5, 1);
            trees *= this.TraverseSlope(7, 1);
            trees *= this.TraverseSlope(1, 2);

            return trees;
        }

        public TobogganTrajectory RenderSilver(int renderEvery = 1)
        {
            return this.RenderSlope(3, 1, "TOBOGGAN TRAJECTORY", renderEvery);
        }

        public TobogganTrajectory RenderGold(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            long product = 1;

            foreach ((int Right, int Down) slope in new[]
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2)
            })
            {
                List<TobogganStep> steps = this.GetSteps(slope.Right, slope.Down);
                int trees = 0;
                HashSet<(int X, int Y)> trail = [];

                for (int i = 0; i < steps.Count; i++)
                {
                    TobogganStep step = steps[i];

                    trail.Add((step.MapX, step.Y));

                    if (step.Tree)
                    {
                        trees++;
                    }

                    if (i % renderEvery == 0 || i == steps.Count - 1)
                    {
                        frames.Add(this.BuildFrame(
                            step,
                            trail,
                            $"SLOPE R{slope.Right} D{slope.Down} // TREES {trees:000} // PRODUCT {product}"));
                    }
                }

                product *= trees;

                TobogganStep last = steps.Last();

                frames.Add(this.BuildFrame(
                    last,
                    trail,
                    $"SLOPE R{slope.Right} D{slope.Down} COMPLETE // TREES {trees:000} // PRODUCT {product}"));

            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private int TraverseSlope(int right, int down)
            => this.GetSteps(right, down).Count(step => step.Tree);

        private TobogganTrajectory RenderSlope(
            int right,
            int down,
            string title,
            int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<TobogganStep> steps = this.GetSteps(right, down);
            List<string[]> frames = [];
            HashSet<(int X, int Y)> trail = [];
            int trees = 0;

            for (int i = 0; i < steps.Count; i++)
            {
                TobogganStep step = steps[i];

                trail.Add((step.RealX, step.Y));

                if (step.Tree)
                {
                    trees++;
                }

                if (i % renderEvery == 0 || i == steps.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        step,
                        trail,
                        $"{title} // RIGHT {right} DOWN {down} // TREES {trees:000}"));
                }
            }

            TobogganStep last = steps.Last();

            frames.Add(this.BuildFrame(
                    last,
                    trail,
                    $"BOTTOM REACHED // RIGHT {right} DOWN {down} // TREES {trees:000}"));

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<TobogganStep> GetSteps(int right, int down)
        {
            List<TobogganStep> steps = [];
            int width = this.Input[0].Length;
            int x = 0;

            for (int y = 0; y < this.Input.Length; y += down)
            {
                int mapX = x % width;
                bool tree = this.Input[y][mapX] == '#';

                steps.Add(new TobogganStep(x, mapX, y, tree));

                x += right;
            }

            return steps;
        }

        private string[] BuildFrame(
            TobogganStep current,
            HashSet<(int X, int Y)> trail,
            string title)
        {
            List<string> result = [];

            int width = this.Input[0].Length;

            int cameraTop = Math.Max(0, current.Y - ViewTopPadding);
            int cameraBottom = Math.Min(this.Input.Length, cameraTop + ViewHeight);

            if (cameraBottom - cameraTop < ViewHeight)
            {
                cameraTop = Math.Max(0, cameraBottom - ViewHeight);
            }

            result.Add(title);
            result.Add($"X {current.RealX:0000} // WRAPPED {current.MapX:00} // Y {current.Y:000}");
            result.Add(string.Empty);

            for (int y = cameraTop; y < cameraBottom; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < width; x++)
                {
                    char value = this.Input[y][x];

                    bool isCurrent =
                        current.MapX == x &&
                        current.Y == y;

                    bool wasVisited =
                        trail.Contains((x, y));

                    if (isCurrent)
                    {
                        sb.Append(current.Tree ? 'X' : '@');
                    }
                    else if (wasVisited)
                    {
                        sb.Append(value == '#' ? 'X' : 'O');
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

        private readonly record struct TobogganStep(
            int RealX,
            int MapX,
            int Y,
            bool Tree);
    }
}