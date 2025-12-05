namespace AdventOfCode.Puzzles._2024.Day_17___Chronospatial_Computer
{
    public class ChronospatialComputer
    {
        public long A { get; private set; }

        public long B { get; private set; }

        public long C { get; private set; }

        public int Pointer { get; private set; }

        public List<int> Program { get; private set; }

        public ChronospatialComputer(string[] input)
        {
            this.Program = [];
            this.Parse(input);
        }

        private long Combo(int operand)
        {
            if (operand == 4)
            {
                return this.A;
            }
            else if (operand == 5)
            {
                return this.B;
            }
            else if (operand == 6)
            {
                return this.C;
            }

            return operand;
        }

        public List<long> Execute(long a = -1)
        {
            this.A = a == -1 ? this.A : a;
            this.Pointer = 0;
            List<long> result = [];
            
            while (true)
            {
                if (this.Pointer > this.Program.Count - 1)
                {
                    break;
                }

                Instruction instruction = (Instruction)this.Program[this.Pointer];
                int operand = this.Program[this.Pointer + 1];
                long combo = this.Combo(operand);
                this.Pointer += 2;

                switch (instruction)
                {
                    case Instruction.Adv:
                        this.A = (long)(this.A / Math.Pow(2, combo));
                        break;
                    case Instruction.Bxl:
                        this.B ^= operand;
                        break;
                    case Instruction.Bst:
                        this.B = combo & 0b111;
                        break;
                    case Instruction.Jnz:
                        if (this.A != 0)
                        {
                            this.Pointer = operand;
                        }
                        break;
                    case Instruction.Bxc:
                        this.B ^= this.C;
                        break;
                    case Instruction.Out:
                        result.Add(combo % 8);
                        break;
                    case Instruction.Bdv:
                        this.B = (long)(this.A / Math.Pow(2, combo));
                        break;
                    case Instruction.Cdv:
                        this.C = (long)(this.A / Math.Pow(2, combo));
                        break;
                }
            }

            return result;
        }

        public string FinalOutput() => string.Join(",", this.Execute());

        public long LowestPostive() => this.FindLowestPostive(0, this.Program.Count - 1);

        public long FindLowestPostive(long value, int index)
        {
            if (index < 0)
            {
                return value;
            }

            long minValue = value * 8;
            long maxValue = value * 8 + 7;

            for (long testValue = minValue; testValue <= maxValue; testValue++)
            {
                if (this.Execute(testValue)[0] == this.Program[index])
                {
                    long result = FindLowestPostive(testValue, index - 1);

                    if (result > 0)
                    {
                        return result;
                    }
                }
            }

            return 0;
        }

        private void Parse(string[] input)
        {
            this.A = int.Parse(input[0].Replace("Register A: ", string.Empty));
            this.B = int.Parse(input[1].Replace("Register B: ", string.Empty));
            this.C = int.Parse(input[2].Replace("Register C: ", string.Empty));

            this.Program = input[4].Replace("Program: ", string.Empty).Split(",").Select(x => int.Parse(x)).ToList();
        }
    }

    public enum Instruction
    { 
        Adv = 0,
        Bxl = 1,
        Bst = 2,
        Jnz = 3,
        Bxc = 4,
        Out = 5,
        Bdv = 6,
        Cdv = 7
    }

}
