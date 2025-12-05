namespace AdventOfCode.Puzzles._2015.Day_07___Some_Assembly_Required
{
    using AdventOfCode.Core;

    public class Instruction
    {
        public Instruction(string input)
        {
            string[] tokens = input.Split(" ");

            if (tokens.Length == 3)
            {
                this.LogicGate = Bitwise.Set;
                this.InputA = new(tokens[0]);
                this.Output = new(tokens[2]);

                return;
            }

            if (tokens[0] == "NOT")
            {
                this.LogicGate = Bitwise.Not;
                this.InputA = new(tokens[1]);
                this.Output = new(tokens[3]);

                return;
            }

            this.InputA = new(tokens[0]);

            switch (tokens[1])
            {
                case "AND":
                    this.LogicGate = Bitwise.And;
                    break;
                case "OR":
                    this.LogicGate = Bitwise.Or;
                    break;
                case "RSHIFT":
                    this.LogicGate = Bitwise.RShift;
                    break;
                case "LSHIFT":
                    this.LogicGate = Bitwise.LShift;
                    break;
            }

            this.InputB = new(tokens[2]);
            this.Output = new(tokens[4]);
        }

        public Bitwise LogicGate { get; }

        public Wire InputA { get; }

        public Wire? InputB { get; }

        public Wire Output { get; }

        public bool IsSetter() => this.LogicGate == Bitwise.Set && !this.InputA.IsAddress;

        public void Print(int key)
        {
            string inputA = this.InputA.IsAddress ? this.InputA.Address : $"{this.InputA.Constant}";
            string inputB = this.InputB?.IsAddress == true ? this.InputB.Address : $"{this.InputB?.Constant}";

            PuzzleConsole.WriteLine($"{key}: {inputA} {this.LogicGate} {inputB} -> {this.Output.Address}");
        }
    }
}
