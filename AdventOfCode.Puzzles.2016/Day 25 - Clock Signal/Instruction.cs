namespace AdventOfCode.Puzzles._2016.Day_25___Clock_Signal
{
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        public Instruction(string line)
        {
            this.Type = ParseInstructionType(line[..3]);

            line = line[3..];
            string[] tokens = line.SplitSpace();

            List<char> registers = new() { 'a', 'b', 'c', 'd' };

            if (char.IsNumber(tokens[0][0]) || tokens[0][0] == '-')
            {
                this.IsRegisterA = false;
                this.ValueA = tokens[0].ToInt();
            }
            else
            {
                this.IsRegisterA = true;
                this.ValueA = registers.IndexOf(tokens[0][0]);
            }

            if (tokens.Length == 1)
            {
                return;
            }

            if (char.IsNumber(tokens[1][0]) || tokens[1][0] == '-')
            {
                this.IsRegisterB = false;
                this.ValueB = tokens[1].ToInt();
            }
            else
            {
                this.IsRegisterB = true;
                this.ValueB = registers.IndexOf(tokens[1][0]);
            }

            if (tokens.Length == 2)
            {
                return;
            }

            if (char.IsNumber(tokens[2][0]) || tokens[2][0] == '-')
            {
                this.IsRegisterC = false;
                this.ValueC = tokens[2].ToInt();
            }
            else
            {
                this.IsRegisterC = true;
                this.ValueC = registers.IndexOf(tokens[2][0]);
            }
        }

        public InstructionType Type { get; set; }

        public bool IsRegisterA { get; }

        public bool IsRegisterB { get; }

        public bool IsRegisterC { get; }

        public long ValueA { get; }

        public long ValueB { get; }

        public long ValueC { get; }

        private static InstructionType ParseInstructionType(string value)
        {
            return value switch
            {
                "tgl" => InstructionType.Tgl,
                "cpy" => InstructionType.Cpy,
                "inc" => InstructionType.Inc,
                "dec" => InstructionType.Dec,
                "jnz" => InstructionType.Jnz,
                "mul" => InstructionType.Mul,
                "out" => InstructionType.Out,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
