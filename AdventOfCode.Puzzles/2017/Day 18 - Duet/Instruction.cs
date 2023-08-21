namespace AdventOfCode.Puzzles._2017.Day_18___Duet
{
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        private static readonly Dictionary<string, InstructionType> StringToInstructionTypeMap = new()
        {
            { "snd", InstructionType.Snd },
            { "set", InstructionType.Set },
            { "add", InstructionType.Add },
            { "mul", InstructionType.Mul },
            { "mod", InstructionType.Mod },
            { "rcv", InstructionType.Rcv },
            { "jgz", InstructionType.Jgz }
        };

        public Instruction(string line)
        {
            this.Type = StringToInstructionTypeMap[line[..3]];

            line = line[3..];
            string[] tokens = line.SplitSpace();

            List<char> registers = new();

            for (int i = 'a'; i <= 'z'; i++)
            {
                registers.Add((char)i);
            }

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
    }
}
