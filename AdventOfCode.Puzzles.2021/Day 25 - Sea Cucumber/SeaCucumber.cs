namespace AdventOfCode.Puzzles._2021.Day_25___Sea_Cucumber
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class SeaCucumber
    {
        private const int Empty = 0;
        private const int East = 1;
        private const int South = 2;

        public SeaCucumber(string[] input)
            => this.Map = new(input, c => ".>v".IndexOf(c));

        public SeaCucumber(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public VectorArray<int, int> Map { get; private set; }

        public IFrameRenderer? Renderer { get; }

        public int Run()
        {
            int step = 0;
            bool moved = true;

            while (moved)
            {
                moved = this.Step();
                step++;
            }

            return step;
        }

        public SeaCucumber RenderSilver(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            int step = 0;
            bool moved = true;

            this.Renderer.RenderFrame(new Frame(this.BuildFrame(step, "INITIAL SCAN")));

            while (moved)
            {
                moved = this.Step();
                step++;

                if (step % renderEvery == 0 || !moved)
                {
                    this.Renderer.RenderFrame(new Frame(
                        this.BuildFrame(
                            step,
                            moved
                                ? "SEA CUCUMBER MIGRATION"
                                : "SEA CUCUMBERS LOCKED")));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                this.Renderer.RenderFrame(new Frame(
                    this.BuildFrame(step, "LANDED // NO MOVEMENT")));
            }

            return this;
        }

        public SeaCucumber RenderGold(int renderEvery = 1)
            => this.RenderSilver(renderEvery);

        private bool Step()
        {
            bool moved = false;

            VectorArray<int, int> state = this.Map.Clone();

            foreach (VectorCell<int, int> cell in this.Map.AxisEnumerator())
            {
                Vector<int> point = cell.Point;
                Vector<int> next = new((point.X + 1) % this.Map.Width, point.Y);

                if (this.Map[point] == East && this.Map[next] == Empty)
                {
                    state[point] = Empty;
                    state[next] = East;
                    moved = true;
                }
            }

            this.Map = state.Clone();

            foreach (VectorCell<int, int> cell in this.Map.AxisEnumerator())
            {
                Vector<int> point = cell.Point;
                Vector<int> next = new(point.X, (point.Y + 1) % this.Map.Height);

                if (this.Map[point] == South && this.Map[next] == Empty)
                {
                    state[point] = Empty;
                    state[next] = South;
                    moved = true;
                }
            }

            this.Map = state.Clone();

            return moved;
        }

        private string[] BuildFrame(int step, string title)
        {
            List<string> result = [];

            result.Add($"{title} // STEP {step:0000}");
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    sb.Append(this.Map[y, x] switch
                    {
                        East => '>',
                        South => 'v',
                        _ => '.'
                    });
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }
    }
}