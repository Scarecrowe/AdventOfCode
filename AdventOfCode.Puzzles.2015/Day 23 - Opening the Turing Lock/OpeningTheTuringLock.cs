namespace AdventOfCode.Puzzles._2015.Day_23___Opening_the_Turing_Lock
{
    using AdventOfCode.Core.Extensions;

    public class OpeningTheTuringLock
    {
        public OpeningTheTuringLock(string[] input, uint registerA = 0)
        {
            this.Registers = new()
            {
                { "a", registerA },
                { "b", 0 }
            };

            this.Instructions = ParseInsturctions(input);
        }

        public Dictionary<string, uint> Registers { get; private set; }

        public Dictionary<int, Instruction> Instructions { get; private set; }

        public OpeningTheTuringLock Execute()
        {
            for (int pointer = 0; pointer < this.Instructions.Count; pointer++)
            {
                Instruction instruction = this.Instructions[pointer];

                switch (instruction.Type)
                {
                    case InstructionType.Half:
                        this.Registers[instruction.Register] = (uint)(this.Registers[instruction.Register] / 2);

                        break;

                    case InstructionType.Triple:
                        this.Registers[instruction.Register] *= 3;
                        break;

                    case InstructionType.Increment:
                        this.Registers[instruction.Register]++;
                        break;

                    case InstructionType.Jump:
                        pointer += (int)instruction.Value - 1;
                        break;

                    case InstructionType.JumpIfEven:
                        if (this.Registers[instruction.Register] % 2 == 0)
                        {
                            pointer += (int)instruction.Value - 1;
                        }

                        break;

                    case InstructionType.JumpIfOne:
                        if (this.Registers[instruction.Register] == 1)
                        {
                            pointer += (int)instruction.Value - 1;
                        }

                        break;
                }
            }

            return this;
        }

        public uint RegisterB() => this.Registers["b"];

        private static Dictionary<int, Instruction> ParseInsturctions(string[] input)
        {
            Dictionary<int, Instruction> result = new();
            int index = 0;

            foreach (string line in input)
            {
                string[] parts = line.SplitSpace();

                InstructionType type = InstructionType.Half;
                string register = string.Empty;
                int value = 0;

                switch (parts[0])
                {
                    case "hlf":
                        register = parts[1];

                        break;
                    case "tpl":
                        type = InstructionType.Triple;
                        register = parts[1];

                        break;
                    case "inc":
                        type = InstructionType.Increment;
                        register = parts[1];

                        break;
                    case "jmp":
                        type = InstructionType.Jump;
                        value = int.Parse(parts[1][1..]);

                        if (parts[1][0] == '-')
                        {
                            value *= -1;
                        }

                        break;
                    case "jie":
                        type = InstructionType.JumpIfEven;
                        register = parts[1].Substring(0, parts[1].Length - 1);
                        value = int.Parse(parts[2][1..]);

                        if (parts[2][0] == '-')
                        {
                            value *= -1;
                        }

                        break;
                    case "jio":
                        type = InstructionType.JumpIfOne;
                        register = parts[1][..^1];
                        value = int.Parse(parts[2][1..]);

                        if (parts[2][0] == '-')
                        {
                            value *= -1;
                        }

                        break;
                }

                result.Add(index, new(type, register, value));

                index++;
            }

            return result;
        }
    }
}
