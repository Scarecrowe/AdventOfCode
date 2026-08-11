namespace AdventOfCode.Puzzles._2015.Day_06___Probably_a_Fire_Hazard
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class ProbablyAFireHazard
    {
        private const int GridSize = 1000;
        private const int VisualWidth = 100;
        private const int VisualHeight = 100;
        private const int HoldFrames = 24;

        public ProbablyAFireHazard(string[] input, LightBrightness brightness)
        {
            this.Input = input;
            this.Brightness = brightness;
            this.Map = new(GridSize, GridSize);
            this.Parse(input, brightness);
        }

        public ProbablyAFireHazard(string[] input, LightBrightness brightness, IFrameRenderer renderer)
            : this(input, brightness)
        {
            this.Renderer = renderer;
        }

        public VectorArray<int, int> Map { get; }

        public IFrameRenderer? Renderer { get; }

        private string[] Input { get; }

        private LightBrightness Brightness { get; }

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

        private readonly record struct VisualInstruction(LightGrid Grid, LightMode Mode, int Index);

        public int Lit() => this.Map.Sum();

        public ProbablyAFireHazard RenderSilver(int renderEvery = 1)
        {
            return this.Render(LightBrightness.Single, renderEvery);
        }

        public ProbablyAFireHazard RenderGold(int renderEvery = 1)
        {
            return this.Render(LightBrightness.Multiple, renderEvery);
        }

        private void Parse(string[] input, LightBrightness brightness)
        {
            foreach (string line in input)
            {
                if (brightness == LightBrightness.Single)
                {
                    this.ParseBrightness(ParseLine(line), this.SingleBrightness);
                    continue;
                }

                this.ParseBrightness(ParseLine(line), this.MultipleBrightness);
            }
        }

        private static (LightGrid Grid, LightMode Mode) ParseLine(string line)
        {
            string[] tokens = line.SplitSpace();

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

        private void ParseBrightness((LightGrid Grid, LightMode Mode) value, Action<Vector<int>, LightMode> action)
        {
            Vector<int>.AxisEnumerator(value.Grid, value.Grid.Width + 1, value.Grid.Height + 1).ForEach(
                point => {
                    action(point, value.Mode);
                });
        }

        private void SingleBrightness(Vector<int> point, LightMode mode)
            => this.Map[point] = this.SingleModifiers[mode](point, this.Map[point]);

        private void MultipleBrightness(Vector<int> point, LightMode mode)
            => this.Map[point] = this.MultiModifiers[mode](point, this.Map[point]).ZeroIfNegative();

        private ProbablyAFireHazard Render(LightBrightness brightness, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            int[,] visual = new int[VisualHeight, VisualWidth];
            List<VisualInstruction> instructions = this.Input
                .Select((line, index) =>
                {
                    (LightGrid grid, LightMode mode) = ParseLine(line);
                    return new VisualInstruction(grid, mode, index + 1);
                })
                .ToList();

            int total = 0;
            int maxValue = brightness == LightBrightness.Single ? 1 : 0;

            this.Renderer.RenderFrame(new Frame(this.BuildFrame(
                visual,
                null,
                0,
                instructions.Count,
                total,
                maxValue,
                brightness)));

            foreach (VisualInstruction instruction in instructions)
            {
                this.ApplyVisualInstruction(visual, instruction, brightness, ref total, ref maxValue);

                if (instruction.Index % renderEvery == 0 || instruction.Index == instructions.Count)
                {
                    this.Renderer.RenderFrame(new Frame(this.BuildFrame(
                        visual,
                        instruction,
                        instruction.Index,
                        instructions.Count,
                        total,
                        maxValue,
                        brightness)));
                }
            }

            VisualInstruction? last = instructions.Count == 0 ? null : instructions.Last();

            for (int i = 0; i < HoldFrames; i++)
            {
                this.Renderer.RenderFrame(new Frame(this.BuildFrame(
                    visual,
                    last,
                    instructions.Count,
                    instructions.Count,
                    total,
                    maxValue,
                    brightness,
                    complete: true)));
            }

            return this;
        }

        private void ApplyVisualInstruction(
            int[,] visual,
            VisualInstruction instruction,
            LightBrightness brightness,
            ref int total,
            ref int maxValue)
        {
            int x1 = Math.Min(instruction.Grid.X, instruction.Grid.Width);
            int x2 = Math.Max(instruction.Grid.X, instruction.Grid.Width);
            int y1 = Math.Min(instruction.Grid.Y, instruction.Grid.Height);
            int y2 = Math.Max(instruction.Grid.Y, instruction.Grid.Height);

            int visualX1 = ScaleDown(x1, VisualWidth);
            int visualX2 = ScaleDown(x2, VisualWidth);
            int visualY1 = ScaleDown(y1, VisualHeight);
            int visualY2 = ScaleDown(y2, VisualHeight);

            for (int y = visualY1; y <= visualY2; y++)
            {
                for (int x = visualX1; x <= visualX2; x++)
                {
                    int previous = visual[y, x];
                    int next = brightness == LightBrightness.Single
                        ? this.ApplySingleVisual(previous, instruction.Mode)
                        : this.ApplyMultipleVisual(previous, instruction.Mode);

                    visual[y, x] = next;
                    total += next - previous;

                    if (next > maxValue)
                    {
                        maxValue = next;
                    }
                }
            }
        }

        private int ApplySingleVisual(int value, LightMode mode)
        {
            return mode switch
            {
                LightMode.Toggle => value.Toggle(),
                LightMode.On => 1,
                LightMode.Off => 0,
                _ => value
            };
        }

        private int ApplyMultipleVisual(int value, LightMode mode)
        {
            return mode switch
            {
                LightMode.Toggle => value + 2,
                LightMode.On => value + 1,
                LightMode.Off => Math.Max(0, value - 1),
                _ => value
            };
        }

        private string[] BuildFrame(
            int[,] visual,
            VisualInstruction? instruction,
            int index,
            int count,
            int total,
            int maxValue,
            LightBrightness brightness,
            bool complete = false)
        {
            List<string> result = [];
            string mode = brightness == LightBrightness.Single ? "SILVER" : "GOLD";
            string title = complete
                ? $"PROBABLY A FIRE HAZARD // {mode} COMPLETE"
                : $"PROBABLY A FIRE HAZARD // {mode}";

            result.Add(title);
            result.Add($"INSTRUCTION {index:000}/{count:000} // VISUAL TOTAL {total:000000} // MAX {maxValue:000}");
            result.Add(instruction == null
                ? "WAITING FOR SANTA'S INSTRUCTIONS"
                : $"{instruction.Value.Mode.ToString().ToUpperInvariant()} {instruction.Value.Grid.X:000},{instruction.Value.Grid.Y:000} THROUGH {instruction.Value.Grid.Width:000},{instruction.Value.Grid.Height:000}");
            result.Add(new string('-', VisualWidth));

            for (int y = 0; y < VisualHeight; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < VisualWidth; x++)
                {
                    bool activeInstruction = instruction != null && this.IsInsideInstruction(x, y, instruction.Value);
                    sb.Append(this.GetVisualCharacter(visual[y, x], maxValue, activeInstruction));
                }

                result.Add(sb.ToString());
            }

            result.Add(new string('-', VisualWidth));
            result.Add("EACH CHARACTER REPRESENTS A 10X10 BLOCK OF LIGHTS");

            return [.. result];
        }

        private bool IsInsideInstruction(int x, int y, VisualInstruction instruction)
        {
            int x1 = ScaleDown(Math.Min(instruction.Grid.X, instruction.Grid.Width), VisualWidth);
            int x2 = ScaleDown(Math.Max(instruction.Grid.X, instruction.Grid.Width), VisualWidth);
            int y1 = ScaleDown(Math.Min(instruction.Grid.Y, instruction.Grid.Height), VisualHeight);
            int y2 = ScaleDown(Math.Max(instruction.Grid.Y, instruction.Grid.Height), VisualHeight);

            return x >= x1 && x <= x2 && y >= y1 && y <= y2;
        }

        private char GetVisualCharacter(int value, int maxValue, bool activeInstruction)
        {
            if (activeInstruction)
            {
                return '@';
            }

            if (value <= 0)
            {
                return ' ';
            }

            if (this.Brightness == LightBrightness.Single)
            {
                return '#';
            }

            if (maxValue <= 0)
            {
                return ' ';
            }

            double ratio = value / (double)maxValue;

            return ratio switch
            {
                < 0.20 => '.',
                < 0.40 => ':',
                < 0.60 => '*',
                < 0.80 => 'O',
                _ => '#'
            };
        }

        private static int ScaleDown(int value, int visualSize)
            => Math.Clamp(value * visualSize / GridSize, 0, visualSize - 1);
    }
}
