namespace AdventOfCode.Puzzles._2018.Day_18___Settlers_of_The_North_Pole
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class SettlersOfTheNorthPole
    {
        public SettlersOfTheNorthPole(string[] input) => this.Map = new(input, ".|#");

        public SettlersOfTheNorthPole(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, EntityType> Map { get; }

        public long Cycle(int minutes)
        {
            List<long> states = new();

            for (int i = 1; i <= minutes; i++)
            {
                VectorArray<int, EntityType> state = this.Map.Clone();

                foreach (VectorCell<int, EntityType> cell in state.AxisEnumerator())
                {
                    List<VectorCell<int, EntityType>> adjacent = state.AdjacentInterCardinal(cell.Point).ToList();

                    switch (cell.Value)
                    {
                        case EntityType.Open:
                            if (adjacent.Count(c => c.Value == EntityType.Trees) >= 3)
                            {
                                this.Map[cell.Point] = EntityType.Trees;
                            }

                            break;
                        case EntityType.Trees:
                            if (adjacent.Count(c => c.Value == EntityType.LumberyYard) >= 3)
                            {
                                this.Map[cell.Point] = EntityType.LumberyYard;
                            }

                            break;
                        case EntityType.LumberyYard:
                            this.Map[cell.Point] = (adjacent.Any(c => c.Value == EntityType.LumberyYard)
                                && adjacent.Any(c => c.Value == EntityType.Trees))
                                ? EntityType.LumberyYard : EntityType.Open;
                            break;
                    }
                }

                states.Add(this.Score());

                for (int j = 1; j < states.Count - 1; j++)
                {
                    if (states[j] == states[^1] && states[j - 1] == states[^2])
                    {
                        int value = states.Count - j - 1;
                        return states[(i - value + ((minutes - i) % value)) - 1];
                    }
                }
            }

            return states[minutes - 1];
        }

        public SettlersOfTheNorthPole RenderSilver()
        {
            return this.RenderGrowth(10, "SETTLERS OF THE NORTH POLE // 10 MINUTES", renderEvery: 1);
        }

        public SettlersOfTheNorthPole RenderGold(int renderEvery = 1)
        {
            return this.RenderGrowth(1000000000, "SETTLERS OF THE NORTH POLE // 1000000000 MINUTES", renderEvery);
        }

        private SettlersOfTheNorthPole RenderGrowth(int minutes, string title, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<long> states = new();
            List<string[]> frames = [];
            int cycleStart = -1;
            int cycleLength = -1;
            int finalMinute = minutes;
            long finalScore = 0;

            frames.Add(this.BuildFrame(title, 0, this.Score(), "INITIAL SCAN"));

            for (int minute = 1; minute <= minutes; minute++)
            {
                this.Tick();

                long score = this.Score();
                states.Add(score);

                if (minute % renderEvery == 0 || minute == minutes)
                {
                    frames.Add(this.BuildFrame(title, minute, score, "MAGIC SHIFTING"));
                }

                for (int j = 1; j < states.Count - 1; j++)
                {
                    if (states[j] == states[^1] && states[j - 1] == states[^2])
                    {
                        cycleStart = j;
                        cycleLength = states.Count - j - 1;
                        finalMinute = minute - cycleLength + ((minutes - minute) % cycleLength);
                        finalScore = states[finalMinute - 1];
                        minute = minutes;
                        break;
                    }
                }
            }

            if (cycleStart >= 0)
            {
                frames.Add(this.BuildFrame(
                    title,
                    finalMinute,
                    finalScore,
                    $"CYCLE FOUND // START {cycleStart:0000} // LENGTH {cycleLength:0000}"));

                frames.Add(this.BuildFrame(
                    title,
                    minutes,
                    finalScore,
                    "JUMPED TO FINAL MINUTE"));
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(title, minutes, cycleStart >= 0 ? finalScore : this.Score(), "RESOURCE VALUE LOCKED"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private void Tick()
        {
            VectorArray<int, EntityType> state = this.Map.Clone();

            foreach (VectorCell<int, EntityType> cell in state.AxisEnumerator())
            {
                List<VectorCell<int, EntityType>> adjacent = state.AdjacentInterCardinal(cell.Point).ToList();

                switch (cell.Value)
                {
                    case EntityType.Open:
                        if (adjacent.Count(c => c.Value == EntityType.Trees) >= 3)
                        {
                            this.Map[cell.Point] = EntityType.Trees;
                        }

                        break;
                    case EntityType.Trees:
                        if (adjacent.Count(c => c.Value == EntityType.LumberyYard) >= 3)
                        {
                            this.Map[cell.Point] = EntityType.LumberyYard;
                        }

                        break;
                    case EntityType.LumberyYard:
                        this.Map[cell.Point] = (adjacent.Any(c => c.Value == EntityType.LumberyYard)
                            && adjacent.Any(c => c.Value == EntityType.Trees))
                            ? EntityType.LumberyYard : EntityType.Open;
                        break;
                }
            }
        }

        private string[] BuildFrame(string title, int minute, long score, string status)
        {
            List<string> result = [];

            result.Add(title);
            result.Add($"MINUTE {minute:0000000000} // TREES {this.Map.Count(EntityType.Trees):0000} // LUMBERYARDS {this.Map.Count(EntityType.LumberyYard):0000} // VALUE {score:0000000000}");
            result.Add(status);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    sb.Append(this.ToChar(this.Map[y, x]));
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private char ToChar(EntityType entity)
        {
            return entity switch
            {
                EntityType.Open => '.',
                EntityType.Trees => '|',
                EntityType.LumberyYard => '#',
                _ => ' '
            };
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

        private long Score() => this.Map.Count(EntityType.Trees) * this.Map.Count(EntityType.LumberyYard);
    }
}
