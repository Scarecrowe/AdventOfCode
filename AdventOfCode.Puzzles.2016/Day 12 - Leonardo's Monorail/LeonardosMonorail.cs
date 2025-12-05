namespace AdventOfCode.Puzzles._2016.Day_12___Leonardo_s_Monorail
{
    public class LeonardosMonorail
    {
        public LeonardosMonorail(string[] input)
        {
            this.Registers = new int[4];
            this.Instructions = input.Select(x => new Instruction(x)).ToList();
        }

        public List<Instruction> Instructions { get; }

        public int[] Registers { get; }

        public LeonardosMonorail Process(int c = 0)
        {
            int i = 0;
            this.Registers[2] = c;

            while (i < this.Instructions.Count)
            {
                Instruction instruction = this.Instructions[i];

                switch (instruction.Type)
                {
                    case InstructionType.Cpy:
                        if (instruction.IsRegister)
                        {
                            this.Registers[instruction.Register] = this.Registers[instruction.Value];
                        }
                        else
                        {
                            this.Registers[instruction.Register] = instruction.Value;
                        }

                        break;
                    case InstructionType.Inc:
                        this.Registers[instruction.Register]++;
                        break;
                    case InstructionType.Dec:
                        this.Registers[instruction.Register]--;
                        break;
                    case InstructionType.Jnz:
                        int value = instruction.Register;

                        if (instruction.IsRegister)
                        {
                            value = this.Registers[instruction.Register];
                        }

                        if (value != 0)
                        {
                            i += instruction.Value;

                            continue;
                        }

                        break;
                }

                i++;
            }

            return this;
        }
    }
}
