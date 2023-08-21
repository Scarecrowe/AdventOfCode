namespace AdventOfCode.Puzzles._2017.Day_8___I_Heard_You_Like_Registers
{
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        private static readonly Dictionary<string, InstructionOperator> StringToInstructionOperatorMap = new()
        {
            { ">", InstructionOperator.GreaterThan },
            { ">=", InstructionOperator.GreaterThanOrEqualTo },
            { "<", InstructionOperator.LessThan },
            { "<=", InstructionOperator.LessThanOrEqualTo },
            { "==", InstructionOperator.EqualTo },
            { "!=", InstructionOperator.NotEqualTo }
        };

        public Instruction(string input)
        {
            string[] instruction = input.Split(" if ");
            string[] tokens = instruction[0].SplitSpace();

            this.RegisterA = tokens[0];

            switch (tokens[1])
            {
                case "inc":
                    this.Type = InstructionType.Inc;
                    break;
                case "dec":
                    this.Type = InstructionType.Dec;
                    break;
            }

            this.Value = tokens[2].ToInt();

            tokens = instruction[1].SplitSpace();

            this.RegisterB = tokens[0];
            this.Operator = StringToInstructionOperatorMap[tokens[1]];
            this.AssertValue = tokens[2].ToInt();
        }

        public string RegisterA { get; }

        public string RegisterB { get; }

        public InstructionType Type { get; }

        public InstructionOperator Operator { get; }

        public int Value { get; }

        public int AssertValue { get; }
    }
}
