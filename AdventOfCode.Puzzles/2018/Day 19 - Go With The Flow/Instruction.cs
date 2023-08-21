namespace AdventOfCode.Puzzles._2018.Day_19___Go_With_The_Flow
{
    public class Instruction
    {
        public Instruction(string line)
        {
            string[] tokens = line.Split(" ");

            this.Type = tokens[0];
            this.A = int.Parse(tokens[1]);
            this.B = int.Parse(tokens[2]);
            this.C = int.Parse(tokens[3]);
        }

        public Instruction(Instruction instruction)
        {
            this.Type = instruction.Type;
            this.A = instruction.A;
            this.B = instruction.B;
            this.C = instruction.C;
        }

        public string Type { get; }

        public int A { get; }

        public int B { get; }

        public int C { get; }

        public Instruction Clone() => new(this);

        public string Print(List<Instruction> program) => $"[{program.IndexOf(this),-2}]  {this.Type} {this.A,-2} {this.B,-2} {this.C,-2}";
    }
}
