namespace AdventOfCode.Puzzles._2016.Day_23___Safe_Cracking
{
    public class SafeCracking
    {
        private static readonly Func<SafeCracking, bool> Cpy = new((safeCracking) =>
        {
            if (safeCracking.Instruction.IsRegisterA)
            {
                safeCracking.Registers[safeCracking.Instruction.ValueB] = safeCracking.Registers[safeCracking.Instruction.ValueA];
            }
            else
            {
                safeCracking.Registers[safeCracking.Instruction.ValueB] = safeCracking.Instruction.ValueA;
            }

            return false;
        });

        private static readonly Func<SafeCracking, bool> Mul = new((clockSignal) =>
        {
            clockSignal.Registers[clockSignal.Instruction.ValueA] = clockSignal.Registers[clockSignal.Instruction.ValueB] * clockSignal.Registers[clockSignal.Instruction.ValueC];

            return false;
        });

        private static readonly Func<SafeCracking, bool> Inc = new((safeCracking) =>
        {
            safeCracking.Registers[safeCracking.Instruction.ValueA]++;

            return false;
        });

        private static readonly Func<SafeCracking, bool> Dec = new((safeCracking) =>
        {
            safeCracking.Registers[safeCracking.Instruction.ValueA]--;

            return false;
        });

        private static readonly Func<SafeCracking, bool> Jnz = new((safeCracking) =>
        {
            long valueA = safeCracking.Instruction.IsRegisterA
                            ? safeCracking.Registers[safeCracking.Instruction.ValueA]
                            : safeCracking.Instruction.ValueA;

            long valueB = safeCracking.Instruction.IsRegisterB
                ? safeCracking.Registers[safeCracking.Instruction.ValueB]
                : safeCracking.Instruction.ValueB;

            if (valueA != 0)
            {
                safeCracking.Pointer += (int)valueB;

                if (safeCracking.Pointer < 0)
                {
                    safeCracking.Pointer = safeCracking.Instructions.Count - safeCracking.Pointer;
                }

                return true;
            }

            return false;
        });

        private static readonly Func<SafeCracking, bool> Tgl = new((safeCracking) =>
        {
            long valueA = safeCracking.Registers[safeCracking.Instruction.ValueA];

            int move = ((int)valueA + safeCracking.Pointer) % safeCracking.Instructions.Count;

            switch (safeCracking.Instructions[move].Type)
            {
                case InstructionType.Cpy:
                    safeCracking.Instructions[move].Type = InstructionType.Jnz;
                    break;
                case InstructionType.Jnz:
                    safeCracking.Instructions[move].Type = InstructionType.Cpy;
                    break;
                case InstructionType.Dec:
                    safeCracking.Instructions[move].Type = InstructionType.Inc;
                    break;
                case InstructionType.Inc:
                    safeCracking.Instructions[move].Type = InstructionType.Dec;
                    break;
                case InstructionType.Tgl:
                    safeCracking.Instructions[move].Type = InstructionType.Inc;
                    break;
            }

            return false;
        });

        public SafeCracking(string[] input)
        {
            this.Registers = new long[4];
            this.Instructions = input.Select(x => new Instruction(x)).ToList();
            this.Instruction = this.Instructions[0];
        }

        public List<Instruction> Instructions { get; }

        public long[] Registers { get; }

        public int Pointer { get; private set; }

        public Instruction Instruction { get; private set; }

        private static Dictionary<InstructionType, Func<SafeCracking, bool>> Operators { get; } = new()
        {
            { InstructionType.Cpy, Cpy },
            { InstructionType.Mul, Mul },
            { InstructionType.Inc, Inc },
            { InstructionType.Dec, Dec },
            { InstructionType.Jnz, Jnz },
            { InstructionType.Tgl, Tgl }
        };

        public SafeCracking Process(int c = 0)
        {
            this.Pointer = 0;
            this.Registers[0] = c;

            while (this.Pointer < this.Instructions.Count)
            {
                this.Instruction = this.Instructions[this.Pointer];
                if (Operators[this.Instruction.Type](this))
                {
                    continue;
                }

                this.Pointer++;
            }

            return this;
        }

        public long Simple() => this.Process(7).Registers[0];

        public long Advanced() => this.Process(12).Registers[0];
    }
}
