namespace AdventOfCode.Puzzles._2016.Day_12___Leonardo_s_Monorail
{
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        public Instruction(string line)
        {
            string instruction = line[..3];
            line = line[3..];
            string[] tokens = line.SplitSpace();

            switch (instruction)
            {
                case "cpy":
                    this.Type = InstructionType.Cpy;

                    if (!char.IsNumber(tokens[0][0]))
                    {
                        this.IsRegister = true;
                        this.Value = Registers.IndexOf(tokens[0][0]);
                    }
                    else
                    {
                        this.Value = tokens[0].ToInt();
                    }

                    this.Register = Registers.IndexOf(tokens[1][0]);
                    break;
                case "inc":
                    this.Type = InstructionType.Inc;
                    this.Register = Registers.IndexOf(tokens[0][0]);
                    break;
                case "dec":
                    this.Type = InstructionType.Dec;
                    this.Register = Registers.IndexOf(tokens[0][0]);
                    break;
                case "jnz":
                    this.Type = InstructionType.Jnz;

                    if (char.IsNumber(tokens[0][0]))
                    {
                        this.Register = tokens[0].ToInt();
                    }
                    else
                    {
                        this.IsRegister = true;
                        this.Register = Registers.IndexOf(tokens[0][0]);
                    }

                    this.Value = tokens[1].ToInt();
                    break;
            }
        }

        public InstructionType Type { get; }

        public int Register { get; }

        public int Value { get; }

        public bool IsRegister { get; }

        private static List<char> Registers { get; } = new() { 'a', 'b', 'c', 'd' };
    }
}
