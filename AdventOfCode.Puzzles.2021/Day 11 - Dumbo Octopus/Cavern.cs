namespace AdventOfCode.Puzzles._2021.Day_11___Dumbo_Octopus
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class Cavern
    {
        public Cavern(string[] input) => this.Map = new(this, input);

        public Cavern(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public DumboOctopusMap Map { get; private set; }

        public Cavern RunFor(int iterations)
        {
            for (int i = 1; i <= iterations; i++)
            {
                this.RunStep();
            }

            return this;
        }

        public int RunUntil()
        {
            int steps = 1;

            while (true)
            {
                if (this.RunStep())
                {
                    return steps;
                }

                if (steps > 300)
                {
                    break;
                }

                steps++;
            }

            return -1;
        }

        public Cavern RenderSilver()
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];

            frames.Add(this.BuildFrame("DUMBO OCTOPUS // BEFORE ANY STEPS"));

            for (int step = 1; step <= 100; step++)
            {
                bool allFlashed = this.RunStep(reset: false);

                frames.Add(this.BuildFrame(
                    $"DUMBO OCTOPUS // STEP {step:000} // FLASHES {this.Map.Flashes:0000}"));

                this.ResetFlashes();

                if (allFlashed)
                {
                    break;
                }
            }

            frames.Add(this.BuildFrame(
                    $"AFTER 100 STEPS // FLASHES {this.Map.Flashes:0000}"));

            this.RenderPaddedFrames(frames);

            return this;
        }

        public Cavern RenderGold()
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];

            frames.Add(this.BuildFrame("DUMBO OCTOPUS // SEARCHING FOR SYNC"));

            int step = 1;

            while (true)
            {
                bool allFlashed = this.RunStep(reset: false);

                frames.Add(this.BuildFrame(
                    allFlashed
                        ? $"SYNCHRONISED FLASH // STEP {step:000}"
                        : $"DUMBO OCTOPUS // STEP {step:000} // FLASHES {this.Map.Flashes:0000}"));

                this.ResetFlashes();

                if (allFlashed)
                {
                    break;
                }

                step++;
            }

            frames.Add(this.BuildFrame($"ALL OCTOPUSES FLASHED // STEP {step:000}"));

            this.RenderPaddedFrames(frames);

            return this;
        }

        private bool RunStep(bool reset = true)
        {
            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width + 1; x++)
                {
                    this.Map[new(x, y)].Increment();
                }
            }

            bool all = this.Map.All(octopus => octopus.Value.Flashed);

            if (reset)
            {
                this.ResetFlashes();
            }

            return all;
        }

        private void ResetFlashes()
        {
            foreach (KeyValuePair<Vector<int>, DumboOctopus> octopus in this.Map)
            {
                octopus.Value.Reset();
            }
        }

        private string[] BuildFrame(string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width + 1; x++)
                {
                    DumboOctopus octopus = this.Map[new(x, y)];

                    sb.Append(octopus.Flashed
                        ? '*'
                        : octopus.EnegryLevel.ToString()[0]);
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