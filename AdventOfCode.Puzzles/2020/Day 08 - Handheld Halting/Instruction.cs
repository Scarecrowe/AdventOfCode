namespace AdventOfCode.Puzzles._2020.Day_08___Handheld_Halting
{
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        public Instruction(string instruction)
        {
            this.Raw = instruction;
            instruction = instruction.Replace(" ");
            this.Code = instruction[..3];
            this.Plus = instruction[3] == '+';
            this.Value = instruction[4..].ToInt();
        }

        public string Code { get; }

        public bool Plus { get; }

        public int Value { get; }

        public string Raw { get; }

        public bool IsNopOrJump => this.Code == "nop" || this.Code == "jmp";

        public string ReverseNopOrJmp() => $"{(this.Code == "nop" ? "jmp" : "nop")} {(this.Plus ? "+" : "-")} {this.Value}";
    }
}
