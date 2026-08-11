namespace AdventOfCode.Puzzles._2016.Day_08___Two_Factor_Authentication
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TwoFactorAuthentication
    {
        public TwoFactorAuthentication(string[] input)
        {
            this.Display = new(50, 6);
            this.Instructions = Parse(input);
        }

        public TwoFactorAuthentication(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public VectorArray<int, int> Display { get; }

        public List<Instruction> Instructions { get; }

        public int Pixels()
        {
            foreach (Instruction instruction in this.Instructions)
            {
                switch (instruction.Type)
                {
                    case InstructionType.Rect:
                        this.FillRect(instruction);
                        break;
                    case InstructionType.RotateRow:
                        this.Rotate(instruction);
                        break;
                    case InstructionType.RotateColumn:
                        this.Rotate(instruction);
                        break;
                }
            }

            return this.Display.Sum();
        }

        public string Print()
        {
            this.Pixels();

            StringBuilder result = new();
            result.AppendLine().AppendLine();

            foreach (VectorCell<int, int> cell in this.Display.AxisEnumerator(() => result.AppendLine()))
            {
                result.Append(this.Display[cell.Point] == 1 ? "#" : " ");
            }

            result.AppendLine();

            return result.ToString();
        }

        public TwoFactorAuthentication RenderSilver(int holdFrames = 24)
            => this.RenderInstructions("TWO-FACTOR AUTHENTICATION // PIXELS", holdFrames);

        public TwoFactorAuthentication RenderGold(int holdFrames = 48)
            => this.RenderInstructions("TWO-FACTOR AUTHENTICATION // CODE", holdFrames);

        private static List<Instruction> Parse(string[] input) => input.Select(x => new Instruction(x)).ToList();

        private void FillRect(Instruction instruction)
            => Vector<int>
            .AxisEnumerator(instruction.Point.X, instruction.Point.Y)
            .ForEach(x => this.Display[x] = 1);

        private void Rotate(Instruction instruction)
        {
            int count = instruction.Type == InstructionType.RotateColumn ? this.Display.Height : this.Display.Width;

            for (int i = 1; i <= instruction.Count; i++)
            {
                VectorArray<int, int> clone = new(this.Display);

                for (int j = 0; j < count; j++)
                {
                    int k = ((j - 1) + count) % count;

                    if (instruction.Type == InstructionType.RotateColumn)
                    {
                        this.Display[j, instruction.Point.X] = clone[k, instruction.Point.X];
                        continue;
                    }

                    this.Display[new(j, instruction.Point.Y)] = clone[new(k, instruction.Point.Y)];
                }
            }
        }

        private TwoFactorAuthentication RenderInstructions(string title, int holdFrames)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            this.Renderer.RenderFrame(new Frame(this.BuildFrame(title, 0, this.Instructions.Count, "BOOTING DISPLAY")));

            for (int i = 0; i < this.Instructions.Count; i++)
            {
                Instruction instruction = this.Instructions[i];

                switch (instruction.Type)
                {
                    case InstructionType.Rect:
                        this.FillRect(instruction);
                        this.Renderer.RenderFrame(new Frame(this.BuildFrame(title, i + 1, this.Instructions.Count, this.Describe(instruction))));
                        break;

                    case InstructionType.RotateRow:
                    case InstructionType.RotateColumn:
                        this.RenderRotation(title, i + 1, instruction);
                        break;
                }
            }

            string complete = $"COMPLETE // {this.Display.Sum():000} PIXELS LIT";

            for (int i = 0; i < holdFrames; i++)
            {
                this.Renderer.RenderFrame(new Frame(this.BuildFrame(title, this.Instructions.Count, this.Instructions.Count, complete)));
            }

            return this;
        }

        private void RenderRotation(string title, int step, Instruction instruction)
        {
            int count = instruction.Type == InstructionType.RotateColumn ? this.Display.Height : this.Display.Width;

            for (int i = 1; i <= instruction.Count; i++)
            {
                VectorArray<int, int> clone = new(this.Display);

                for (int j = 0; j < count; j++)
                {
                    int k = ((j - 1) + count) % count;

                    if (instruction.Type == InstructionType.RotateColumn)
                    {
                        this.Display[j, instruction.Point.X] = clone[k, instruction.Point.X];
                        continue;
                    }

                    this.Display[new(j, instruction.Point.Y)] = clone[new(k, instruction.Point.Y)];
                }

                this.Renderer?.RenderFrame(new Frame(this.BuildFrame(title, step, this.Instructions.Count, $"{this.Describe(instruction)} // SHIFT {i:00}/{instruction.Count:00}")));
            }
        }

        private string[] BuildFrame(string title, int step, int total, string status)
        {
            List<string> result = [];

            result.Add(title);
            result.Add($"STEP {step:000}/{total:000} // {status}");
            result.Add(string.Empty);

            for (int y = 0; y < this.Display.Height; y++)
            {
                StringBuilder row = new();

                for (int x = 0; x < this.Display.Width; x++)
                {
                    row.Append(this.Display[y, x] == 1 ? '#' : '.');
                }

                result.Add(row.ToString());
            }

            return [.. result];
        }

        private string Describe(Instruction instruction)
            => instruction.Type switch
            {
                InstructionType.Rect => $"RECT {instruction.Point.X}X{instruction.Point.Y}",
                InstructionType.RotateRow => $"ROTATE ROW Y={instruction.Point.Y} BY {instruction.Count}",
                InstructionType.RotateColumn => $"ROTATE COLUMN X={instruction.Point.X} BY {instruction.Count}",
                _ => string.Empty
            };
    }
}
