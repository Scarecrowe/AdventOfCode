namespace AdventOfCode.Puzzles._2016.Day_25___Clock_Signal
{
    using AdventOfCode.Core.Extensions;

    public class ClockSignal
    {
        private static readonly Func<ClockSignal, bool> Out = new((clockSignal) =>
        {
            clockSignal.Clock.Add(clockSignal.Registers[clockSignal.Instruction.ValueA]);

            return false;
        });

        private static readonly Func<ClockSignal, bool> Cpy = new((clockSignal) =>
        {
            if (clockSignal.Instruction.IsRegisterA)
            {
                clockSignal.Registers[clockSignal.Instruction.ValueB] = clockSignal.Registers[clockSignal.Instruction.ValueA];
            }
            else
            {
                clockSignal.Registers[clockSignal.Instruction.ValueB] = clockSignal.Instruction.ValueA;
            }

            return false;
        });

        private static readonly Func<ClockSignal, bool> Mul = new((clockSignal) =>
        {
            clockSignal.Registers[clockSignal.Instruction.ValueA] = clockSignal.Registers[clockSignal.Instruction.ValueB] * clockSignal.Registers[clockSignal.Instruction.ValueC];

            return false;
        });

        private static readonly Func<ClockSignal, bool> Inc = new((clockSignal) =>
        {
            clockSignal.Registers[clockSignal.Instruction.ValueA]++;

            return false;
        });

        private static readonly Func<ClockSignal, bool> Dec = new((clockSignal) =>
        {
            clockSignal.Registers[clockSignal.Instruction.ValueA]--;

            return false;
        });

        private static readonly Func<ClockSignal, bool> Jnz = new((clockSignal) =>
        {
            long valueA = clockSignal.Instruction.IsRegisterA
                            ? clockSignal.Registers[clockSignal.Instruction.ValueA]
                            : clockSignal.Instruction.ValueA;

            long valueB = clockSignal.Instruction.IsRegisterB
                ? clockSignal.Registers[clockSignal.Instruction.ValueB]
                : clockSignal.Instruction.ValueB;

            if (valueA != 0)
            {
                clockSignal.Pointer += (int)valueB;

                if (clockSignal.Pointer < 0)
                {
                    clockSignal.Pointer = clockSignal.Instructions.Count - clockSignal.Pointer;
                }

                return true;
            }

            return false;
        });

        private static readonly Func<ClockSignal, bool> Tgl = new((clockSignal) =>
        {
            long valueA = clockSignal.Registers[clockSignal.Instruction.ValueA];

            int move = ((int)valueA + clockSignal.Pointer) % clockSignal.Instructions.Count;

            switch (clockSignal.Instructions[move].Type)
            {
                case InstructionType.Cpy:
                    clockSignal.Instructions[move].Type = InstructionType.Jnz;
                    break;
                case InstructionType.Jnz:
                    clockSignal.Instructions[move].Type = InstructionType.Cpy;
                    break;
                case InstructionType.Dec:
                    clockSignal.Instructions[move].Type = InstructionType.Inc;
                    break;
                case InstructionType.Inc:
                    clockSignal.Instructions[move].Type = InstructionType.Dec;
                    break;
                case InstructionType.Tgl:
                    clockSignal.Instructions[move].Type = InstructionType.Inc;
                    break;
            }

            return false;
        });

        public ClockSignal(string[] input)
        {
            this.Instructions = input.Select(x => new Instruction(x)).ToList();
            this.Instruction = this.Instructions[0];
            this.Registers = Array.Empty<long>();
            this.Clock = new();
        }

        public List<Instruction> Instructions { get; }

        public long[] Registers { get; private set; }

        public int Pointer { get; private set; }

        public Instruction Instruction { get; private set; }

        public List<long> Clock { get; private set; }

        private static Dictionary<InstructionType, Func<ClockSignal, bool>> Operators { get; } = new()
        {
            { InstructionType.Out, Out },
            { InstructionType.Cpy, Cpy },
            { InstructionType.Mul, Mul },
            { InstructionType.Inc, Inc },
            { InstructionType.Dec, Dec },
            { InstructionType.Jnz, Jnz },
            { InstructionType.Tgl, Tgl }
        };

        public ClockSignal Process(int c = 0)
        {
            this.Registers = new long[4];
            this.Clock = new();
            this.Registers[0] = c;
            this.Pointer = 0;

            while (this.Pointer < this.Instructions.Count)
            {
                this.Instruction = this.Instructions[this.Pointer];
                if (Operators[this.Instruction.Type](this))
                {
                    continue;
                }

                if (this.Clock.Count == 8)
                {
                    return this;
                }

                this.Pointer++;
            }

            return this;
        }

        public int LowesetPositiveInt()
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                this.Process(i);

                if (this.Clock.Join() == "01010101")
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
