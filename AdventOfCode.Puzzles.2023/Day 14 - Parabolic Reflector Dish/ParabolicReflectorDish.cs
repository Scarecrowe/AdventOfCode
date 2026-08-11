namespace AdventOfCode.Puzzles._2023.Day_14___Parabolic_Reflector_Dish
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class ParabolicReflectorDish
    {
        private const long GoldCycles = 1000000000;

        public ParabolicReflectorDish(string[] input)
        {
            this.Map = new(input, c => c);
        }

        public ParabolicReflectorDish(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, char> Map { get; }

        public int Silver()
        {
            this.Tilt(Cardinal.North);
            return this.Load();
        }

        public int Gold()
        {
            return this.RunCycles(GoldCycles);
        }

        public ParabolicReflectorDish RenderSilver(int renderEveryMove = 12, int settleFrames = 18)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            int move = 0;

            this.RenderFrame("PARABOLIC REFLECTOR DISH // TILT NORTH // LOAD ----");
            this.Tilt(Cardinal.North, () =>
            {
                move++;

                if (move % renderEveryMove == 0)
                {
                    this.RenderFrame($"PARABOLIC REFLECTOR DISH // TILT NORTH // MOVE {move:00000} // LOAD {this.Load():000000}");
                }
            });

            int load = this.Load();

            for (int i = 0; i < settleFrames; i++)
            {
                this.RenderFrame($"NORTH SUPPORT LOAD // {load:000000}");
            }

            return this;
        }

        public ParabolicReflectorDish RenderGold(int renderEveryCycle = 1, int settleFrames = 24)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            Dictionary<string, int> seen = new();
            List<int> loads = [];
            int cycle = 0;

            this.RenderFrame("PARABOLIC REFLECTOR DISH // SPIN CYCLE // CYCLE 0000000000 // LOAD ----");

            while (true)
            {
                cycle++;

                this.Tilt(Cardinal.North);
                this.RenderFrame($"SPIN CYCLE // {cycle:0000000000} // NORTH // LOAD {this.Load():000000}");

                this.Tilt(Cardinal.West);
                this.RenderFrame($"SPIN CYCLE // {cycle:0000000000} // WEST  // LOAD {this.Load():000000}");

                this.Tilt(Cardinal.South);
                this.RenderFrame($"SPIN CYCLE // {cycle:0000000000} // SOUTH // LOAD {this.Load():000000}");

                this.Tilt(Cardinal.East);

                string state = this.Map.ToString(c => c);
                int load = this.Load();
                loads.Add(load);

                if (cycle % renderEveryCycle == 0)
                {
                    this.RenderFrame($"SPIN CYCLE // {cycle:0000000000} // EAST  // LOAD {load:000000}");
                }

                if (seen.TryGetValue(state, out int previousCycle))
                {
                    int cycleLength = cycle - previousCycle;
                    long targetCycle = previousCycle + ((GoldCycles - previousCycle) % cycleLength);

                    if (targetCycle == 0)
                    {
                        targetCycle = cycleLength;
                    }

                    int finalLoad = loads[(int)targetCycle - 1];

                    for (int i = 0; i < settleFrames; i++)
                    {
                        this.RenderFrame($"CYCLE DETECTED // START {previousCycle:0000} // LENGTH {cycleLength:0000} // FINAL LOAD {finalLoad:000000}");
                    }

                    return this;
                }

                seen[state] = cycle;
            }
        }

        private int RunCycles(long cycles)
        {
            Dictionary<string, int> seen = new();
            List<int> loads = [];
            int cycle = 0;

            while (true)
            {
                cycle++;

                this.Tilt(Cardinal.North);
                this.Tilt(Cardinal.West);
                this.Tilt(Cardinal.South);
                this.Tilt(Cardinal.East);

                string state = this.Map.ToString(c => c);
                int load = this.Load();
                loads.Add(load);

                if (seen.TryGetValue(state, out int previousCycle))
                {
                    int cycleLength = cycle - previousCycle;
                    long targetCycle = previousCycle + ((cycles - previousCycle) % cycleLength);

                    if (targetCycle == 0)
                    {
                        targetCycle = cycleLength;
                    }

                    return loads[(int)targetCycle - 1];
                }

                seen[state] = cycle;
            }
        }

        private int Load() => this.Map.Values('O').Sum(x => this.Map.Height - x.Point.Y);

        private void Tilt(Cardinal direction, Action? onMove = null)
        {
            var enumerator = this.Map.AxisEnumerator();

            if (direction == Cardinal.East
                || direction == Cardinal.South)
            {
                enumerator = enumerator.Reverse();
            }

            foreach (var cell in enumerator)
            {
                if (cell.Value == 'O')
                {
                    var point = cell.Point;

                    while (true)
                    {
                        var adjacent = this.Map.AdjacentCardinal(point);

                        if (adjacent.Any(x => x.Direction == direction))
                        {
                            var tilt = adjacent.FirstOrDefault(x => x.Direction == direction);

                            if (tilt?.Value == '.')
                            {
                                this.Map[point] = '.';
                                this.Map[tilt.Point] = 'O';
                                point = tilt.Point;
                                onMove?.Invoke();
                                continue;
                            }

                            if (tilt?.Value == 'O'
                                || tilt?.Value == '#')
                            {
                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }

        private void RenderFrame(string title)
        {
            if (this.Renderer == null)
            {
                return;
            }

            this.Renderer.RenderFrame(new Frame(this.BuildFrame(title)));
        }

        private string[] BuildFrame(string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add($"ROUND ROCKS {this.Map.Values('O').Count():0000} // LOAD {this.Load():000000}");
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    sb.Append(this.Map[y, x]);
                }

                result.Add(sb.ToString());
            }

            return PadFrame([.. result], this.FrameWidth(), this.FrameHeight());
        }

        private int FrameWidth()
            => Math.Max(
                this.Map.Width,
                "CYCLE DETECTED // START 0000 // LENGTH 0000 // FINAL LOAD 000000".Length);

        private int FrameHeight()
            => this.Map.Height + 3;

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
