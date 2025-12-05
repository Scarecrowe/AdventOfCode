namespace AdventOfCode.Puzzles._2016.Day_08___Two_Factor_Authentication
{
    using System.Text;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TwoFactorAuthentication
    {
        public TwoFactorAuthentication(string[] input)
        {
            this.Display = new(50, 6);
            this.Instructions = Parse(input);
        }

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

            foreach(VectorCell<int, int> cell in this.Display.AxisEnumerator(() => result.AppendLine()))
            {
                result.Append(this.Display[cell.Point] == 1 ? "#" : " ");
            }

            result.AppendLine();

            return result.ToString();
        }

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
    }
}
