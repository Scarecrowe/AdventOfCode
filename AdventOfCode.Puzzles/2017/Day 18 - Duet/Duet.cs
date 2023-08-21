namespace AdventOfCode.Puzzles._2017.Day_18___Duet
{
    public class Duet
    {
        private static readonly Func<Duet, long?> Rcv = new((duet) =>
        {
            if (duet.Cpu != null)
            {
                int i = 0;

                while (duet.Data.Count == 0 && duet.Cpu != null)
                {
                    i++;

                    if (i > 2)
                    {
                        return duet.Count;
                    }

                    if (duet.Instruction.Type == InstructionType.Rcv
                    && duet.Data.Count == 0
                    && duet.Cpu.Instruction.Type == InstructionType.Rcv
                    && duet.Cpu.Data.Count == 0)
                    {
                        return duet.Count;
                    }

                    Thread.Sleep(100);
                }

                duet.Registers[duet.Instruction.ValueA] = duet.Data.Dequeue();

                return null;
            }

            long last = duet.Data.Last();

            return last > 0 ? last : null;
        });

        private static readonly Func<Duet, long?> Snd = new((duet) =>
        {
            duet.Count++;

            long valueA = duet.Instruction.IsRegisterA
               ? duet.Registers[duet.Instruction.ValueA]
               : duet.Instruction.ValueA;

            if (duet.Cpu == null)
            {
                duet.Data.Enqueue(valueA);
            }
            else
            {
                duet.Cpu.Data.Enqueue(valueA);
            }

            return null;
        });

        private static readonly Func<Duet, long?> Set = new((duet) =>
        {
            long valueB = duet.Instruction.IsRegisterB
                            ? duet.Registers[duet.Instruction.ValueB]
                            : duet.Instruction.ValueB;

            duet.Registers[duet.Instruction.ValueA] = valueB;

            return null;
        });

        private static readonly Func<Duet, long?> Add = new((duet) =>
        {
            long valueB = duet.Instruction.IsRegisterB
                            ? duet.Registers[duet.Instruction.ValueB]
                            : duet.Instruction.ValueB;

            duet.Registers[duet.Instruction.ValueA] += valueB;

            return null;
        });

        private static readonly Func<Duet, long?> Mul = new((duet) =>
        {
            long valueB = duet.Instruction.IsRegisterB
                           ? duet.Registers[duet.Instruction.ValueB]
                           : duet.Instruction.ValueB;

            duet.Registers[duet.Instruction.ValueA] *= valueB;

            return null;
        });

        private static readonly Func<Duet, long?> Mod = new((duet) =>
        {
            long valueB = duet.Instruction.IsRegisterB
                            ? duet.Registers[duet.Instruction.ValueB]
                            : duet.Instruction.ValueB;

            duet.Registers[duet.Instruction.ValueA] %= valueB;

            return null;
        });

        private static readonly Func<Duet, long?> Jgz = new((duet) =>
        {
            long? valueA = duet.Instruction.IsRegisterA
                ? duet.Registers[duet.Instruction.ValueA]
                : duet.Instruction.ValueA;

            long valueB = duet.Instruction.IsRegisterB
                ? duet.Registers[duet.Instruction.ValueB]
                : duet.Instruction.ValueB;

            if (valueA > 0)
            {
                duet.Pointer += (int)valueB;

                if (duet.Pointer < 0)
                {
                    duet.Pointer = duet.Instructions.Count - duet.Pointer;
                }

                return -1;
            }

            return null;
        });

        private static readonly Dictionary<InstructionType, Func<Duet, long?>> Operators = new()
        {
            { InstructionType.Snd, Snd },
            { InstructionType.Set, Set },
            { InstructionType.Add, Add },
            { InstructionType.Mul, Mul },
            { InstructionType.Mod, Mod },
            { InstructionType.Rcv, Rcv },
            { InstructionType.Jgz, Jgz }
        };

        public Duet(string[] input, int programId)
        {
            this.Data = new();
            this.Registers = new long[26];
            this.Registers['p' - 'a'] = programId;
            this.Instructions = input.Select(x => new Instruction(x)).ToList();
            this.Instruction = this.Instructions[0];
            this.Pointer = 0;
            this.Count = 0;
        }

        public List<Instruction> Instructions { get; }

        public long[] Registers { get; }

        public Queue<long> Data { get; }

        public int Pointer { get; private set; }

        public int Count { get; private set; }

        public Instruction Instruction { get; private set; }

        public Duet? Cpu { get; private set; }

        public long Process(Duet? cpu)
        {
            this.Pointer = 0;
            this.Count = 0;
            this.Cpu = cpu;

            while (this.Pointer < this.Instructions.Count)
            {
                Instruction instruction = this.Instructions[this.Pointer];
                this.Instruction = instruction;

                long? result = Operators[this.Instruction.Type](this);

                if (result > 0)
                {
                    return result.Value;
                }

                if (result == -1)
                {
                    continue;
                }

                this.Pointer++;
            }

            return this.Count;
        }
    }
}
