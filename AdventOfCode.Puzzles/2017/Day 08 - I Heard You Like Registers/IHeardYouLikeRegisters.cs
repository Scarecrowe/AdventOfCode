namespace AdventOfCode.Puzzles._2017.Day_8___I_Heard_You_Like_Registers
{
    public class IHeardYouLikeRegisters
    {
        private static readonly Func<IHeardYouLikeRegisters, bool> GreaterThan = new((cpu) =>
        {
            if (cpu.Memory[cpu.Instruction.RegisterB] > cpu.Instruction.AssertValue)
            {
                return true;
            }

            return false;
        });

        private static readonly Func<IHeardYouLikeRegisters, bool> GreaterThanOrEqualTo = new((cpu) =>
        {
            if (cpu.Memory[cpu.Instruction.RegisterB] >= cpu.Instruction.AssertValue)
            {
                return true;
            }

            return false;
        });

        private static readonly Func<IHeardYouLikeRegisters, bool> LessThan = new((cpu) =>
        {
            if (cpu.Memory[cpu.Instruction.RegisterB] < cpu.Instruction.AssertValue)
            {
                return true;
            }

            return false;
        });

        private static readonly Func<IHeardYouLikeRegisters, bool> LessThanOrEqualTo = new((cpu) =>
        {
            if (cpu.Memory[cpu.Instruction.RegisterB] <= cpu.Instruction.AssertValue)
            {
                return true;
            }

            return false;
        });

        private static readonly Func<IHeardYouLikeRegisters, bool> EqualTo = new((cpu) =>
        {
            if (cpu.Memory[cpu.Instruction.RegisterB] == cpu.Instruction.AssertValue)
            {
                return true;
            }

            return false;
        });

        private static readonly Func<IHeardYouLikeRegisters, bool> NotEqualTo = new((cpu) =>
        {
            if (cpu.Memory[cpu.Instruction.RegisterB] != cpu.Instruction.AssertValue)
            {
                return true;
            }

            return false;
        });

        private static readonly Dictionary<InstructionOperator, Func<IHeardYouLikeRegisters, bool>> Operators = new()
        {
            { InstructionOperator.GreaterThan, GreaterThan },
            { InstructionOperator.GreaterThanOrEqualTo, GreaterThanOrEqualTo },
            { InstructionOperator.LessThan, LessThan },
            { InstructionOperator.LessThanOrEqualTo, LessThanOrEqualTo },
            { InstructionOperator.EqualTo, EqualTo },
            { InstructionOperator.NotEqualTo, NotEqualTo },
        };

        public IHeardYouLikeRegisters(string[] instructions)
        {
            this.Instructions = instructions.Select(x => new Instruction(x)).ToList();
            this.Instruction = this.Instructions[0];
            this.Memory = new();
        }

        public List<Instruction> Instructions { get; }

        public Dictionary<string, int> Memory { get; }

        public Instruction Instruction { get; private set; }

        public int Size { get; private set; }

        public IHeardYouLikeRegisters Run()
        {
            foreach (Instruction instruction in this.Instructions)
            {
                this.Instruction = instruction;
                this.AddRegister(instruction.RegisterA);
                this.AddRegister(instruction.RegisterB);

                if (Operators[instruction.Operator](this))
                {
                    this.SetRegister(instruction);
                    this.SetSize(instruction);
                }
            }

            return this;
        }

        public int Max() => this.Memory.Max(x => x.Value);

        private void SetRegister(Instruction instruction)
        {
            switch (instruction.Type)
            {
                case InstructionType.Inc:
                    this.Memory[instruction.RegisterA] += instruction.Value;
                    break;
                case InstructionType.Dec:
                    this.Memory[instruction.RegisterA] -= instruction.Value;
                    break;
            }
        }

        private void SetSize(Instruction instruction)
        {
            if (this.Memory[instruction.RegisterA] > this.Size)
            {
                this.Size = this.Memory[instruction.RegisterA];
            }
        }

        private void AddRegister(string register)
        {
            if (!this.Memory.ContainsKey(register))
            {
                this.Memory.Add(register, 0);
            }
        }
    }
}
