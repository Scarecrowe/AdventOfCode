namespace AdventOfCode.Puzzles._2015.Day_23___Opening_the_Turing_Lock
{
    public class Instruction
    {
        public Instruction(
            InstructionType type,
            string register,
            int value)
        {
            this.Type = type;
            this.Register = register;
            this.Value = value;
        }

        public InstructionType Type { get; }

        public string Register { get; }

        public int Value { get; }
    }
}
