namespace AdventOfCode.Puzzles._2015.Day_06___Probably_a_Fire_Hazard
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ProbablyAFireHazard
    {
        public ProbablyAFireHazard(string[] input, LightBrightness brightness)
        {
            this.Map = new(1000, 1000);

            foreach (string line in input)
            {
                if (brightness == LightBrightness.Single)
                {
                    ParseBrightness(Parse(line), this.SingleBrightness);
                    continue;
                }

                ParseBrightness(Parse(line), this.MultipleBrightness);
            }
        }

        public VectorArray<int, int> Map { get; }

        private Dictionary<LightMode, Func<Vector<int>, int, int>> SingleModifiers { get; } = new()
        {
            { LightMode.Toggle, (point, value) => value.Toggle() },
            { LightMode.On, (point, value) => 1 },
            { LightMode.Off, (point, value) => 0 }
        };

        private Dictionary<LightMode, Func<Vector<int>, int, int>> MultiModifiers { get; } = new()
        {
            { LightMode.Toggle, (point, value) => value + 2 },
            { LightMode.On, (point, value) => value + 1 },
            { LightMode.Off, (point, value) => value - 1 }
        };

        public int Lit() => this.Map.Sum();

        private static (LightGrid Grid, LightMode Mode) Parse(string input)
        {
            string[] tokens = input.SplitSpace();

            int startIndex = 1;
            int sizeIndex = 3;
            LightMode mode = LightMode.Toggle;

            if (tokens[1] == "on" || tokens[1] == "off")
            {
                startIndex = 2;
                sizeIndex = 4;

                mode = tokens[1] == "on" ? LightMode.On : LightMode.Off;
            }

            int[] start = tokens[startIndex].SplitComma().ToInt();
            int[] size = tokens[sizeIndex].SplitComma().ToInt();

            return (new LightGrid(new(start[0], start[1]), size[0], size[1]), mode);
        }

        private static void ParseBrightness((LightGrid Grid, LightMode Mode) value, Action<Vector<int>, LightMode> action)
            => Vector<int>.AxisEnumerator(value.Grid, value.Grid.Width + 1, value.Grid.Height + 1).ForEach(point => action(point, value.Mode));

        private void SingleBrightness(Vector<int> point, LightMode mode)
            => this.Map[point] = this.SingleModifiers[mode](point, this.Map[point]);

        private void MultipleBrightness(Vector<int> point, LightMode mode)
            => this.Map[point] = this.MultiModifiers[mode](point, this.Map[point]).ZeroIfNegative();
    }
}
