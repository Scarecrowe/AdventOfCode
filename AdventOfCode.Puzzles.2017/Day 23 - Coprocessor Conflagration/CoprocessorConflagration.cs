namespace AdventOfCode.Puzzles._2017.Day_23___Coprocessor_Conflagration
{
    public class CoprocessorConflagration
    {
        private static readonly Func<CoprocessorConflagration, bool> Set = new((cpu) =>
        {
            long valueB = cpu.Instruction.IsRegisterB
                           ? cpu.Registers[cpu.Instruction.ValueB]
                           : cpu.Instruction.ValueB;

            cpu.Registers[cpu.Instruction.ValueA] = valueB;

            return false;
        });

        private static readonly Func<CoprocessorConflagration, bool> Sub = new((cpu) =>
        {
            long valueB = cpu.Instruction.IsRegisterB
                           ? cpu.Registers[cpu.Instruction.ValueB]
                           : cpu.Instruction.ValueB;

            cpu.Registers[cpu.Instruction.ValueA] -= valueB;

            return false;
        });

        private static readonly Func<CoprocessorConflagration, bool> Mul = new((cpu) =>
        {
            long valueB = cpu.Instruction.IsRegisterB
                           ? cpu.Registers[cpu.Instruction.ValueB]
                           : cpu.Instruction.ValueB;

            cpu.Registers[cpu.Instruction.ValueA] *= valueB;
            cpu.Count++;

            return false;
        });

        private static readonly Func<CoprocessorConflagration, bool> Jnz = new((cpu) =>
        {
            long valueA = cpu.Instruction.IsRegisterA
                            ? cpu.Registers[cpu.Instruction.ValueA]
                            : cpu.Instruction.ValueA;

            long valueB = cpu.Instruction.IsRegisterB
                ? cpu.Registers[cpu.Instruction.ValueB]
                : cpu.Instruction.ValueB;

            if (valueA != 0)
            {
                cpu.Pointer += (int)valueB;

                if (cpu.Pointer < 0)
                {
                    cpu.Pointer = cpu.Instructions.Count - cpu.Pointer;
                }

                return true;
            }

            return false;
        });

        private static readonly Dictionary<InstructionType, Func<CoprocessorConflagration, bool>> Operators = new()
        {
            { InstructionType.Sub, Sub },
            { InstructionType.Set, Set },
            { InstructionType.Mul, Mul },
            { InstructionType.Jnz, Jnz }
        };

        public CoprocessorConflagration(string[] input, long registerA = 0)
        {
            this.Registers = new long[26];
            this.Registers[0] = registerA;
            this.Instructions = Parse(input);
            this.Instruction = this.Instructions[0];
            this.Pointer = 0;
            this.Count = 0;
        }

        public List<Instruction> Instructions { get; }

        public long[] Registers { get; }

        public int Pointer { get; private set; }

        public int Count { get; private set; }

        public Instruction Instruction { get; private set; }

        public long Simple()
        {
            this.Pointer = 0;
            this.Count = 0;

            while (this.Pointer < this.Instructions.Count)
            {
                this.Instruction = this.Instructions[this.Pointer];
                if (Operators[this.Instruction.Type](this))
                {
                    continue;
                }

                this.Pointer++;
            }

            return this.Count;
        }

        public long Advanced()
        {
            long b = this.Instructions.First(x => x.NameA == "b" && x.Type == InstructionType.Set).ValueB;
            long mul = this.Instructions.First(x => x.NameA == "b" && x.Type == InstructionType.Mul).ValueB;
            long sub = this.Instructions.First(x => x.NameA == "b" && x.Type == InstructionType.Sub).ValueB * -1;
            long subC = this.Instructions.First(x => x.NameA == "c" && x.Type == InstructionType.Sub).ValueB * -1;

            long[] registers = new long[26];

            registers['a' - 'a'] = 1;
            registers['b' - 'a'] = b;
            registers['c' - 'a'] = registers['b' - 'a'];

            if (registers['a' - 'a'] != 0)
            {
                registers['b' - 'a'] = (registers['b' - 'a'] * mul) + sub;
                registers['c' - 'a'] = registers['b' - 'a'] + subC;
            }

            do
            {
                registers['f' - 'a'] = 1;
                registers['d' - 'a'] = 2;
                registers['e' - 'a'] = 3;

                for (registers['d' - 'a'] = 2; registers['d' - 'a'] * registers['d' - 'a'] <= registers['b' - 'a']; registers['d' - 'a']++)
                {
                    if (registers['b' - 'a'] % registers['d' - 'a'] == 0)
                    {
                        registers['f' - 'a'] = 0;
                        break;
                    }
                }

                if (registers['f' - 'a'] == 0)
                {
                    registers['h' - 'a']++;
                }

                registers['g' - 'a'] = registers['b' - 'a'] - registers['c' - 'a'];
                registers['b' - 'a'] += 17;
            }
            while (registers['g' - 'a'] != 0);

            return registers['h' - 'a'];
        }

        private static List<Instruction> Parse(string[] input) => input.Select(x => new Instruction(x)).ToList();
    }
}
