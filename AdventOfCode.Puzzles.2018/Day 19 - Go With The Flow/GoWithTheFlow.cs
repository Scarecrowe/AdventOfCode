namespace AdventOfCode.Puzzles._2018.Day_19___Go_With_The_Flow
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class GoWithTheFlow
    {
        public GoWithTheFlow(string[] input)
        {
            this.Program = new();

            foreach (string line in input.Skip(1))
            {
                this.Program.Add(new(line));
            }

            this.OpCodes = this.OpCodes = new()
                {
                    { "addr", this.Addr },
                    { "addi", this.Addi },
                    { "mulr", this.Mulr },
                    { "muli", this.Muli },
                    { "banr", this.Banr },
                    { "bani", this.Bani },
                    { "borr", this.Borr },
                    { "bori", this.Bori },
                    { "setr", this.Setr },
                    { "seti", this.Seti },
                    { "gtir", this.Gtir },
                    { "gtri", this.Gtri },
                    { "gtrr", this.Gtrr },
                    { "eqir", this.Eqir },
                    { "eqri", this.Eqri },
                    { "eqrr", this.Eqrr }
                };

            this.Registers = new int[6];

            this.InstructionRegister = input[0].Replace("#ip ").ToInt();
        }

        public Dictionary<string, Action<int, int, int>> OpCodes { get; private set; }

        public int[] Registers { get; private set; }

        public List<Instruction> Program { get; }

        public int InstructionPointer { get; private set; }

        public int InstructionRegister { get; }

        public static long Calculate() => GenerateNumber().Factors().Sum();

        public static int Decomplied()
        {
            int a = 1;
            int c = 0;
            int d = 0;
            int f = 0;

            d += 16; // 0
            d++;
            c += 2;  // 17
            d++;
            c *= c;  // 18
            d++;
            c *= d;  // 19
            d++;
            c *= 11; // 20
            d++;
            f += 3;  // 21
            d++;
            f *= d; // 22
            d++;
            f += 3; // 23
            d++;
            c += f; // 24
            d++;
            d += a; // 25
            d++;
            f = d; // 27
            d++;
            f *= d; // 28
            d++;
            f += d;
            d++;
            f *= d;
            d++;
            f *= 14;
            d++;
            f *= d;
            d++;
            c += f;
            d++;
            a = 0;
            d = 0;
            d++;

            int b = 1;
            d++;
            int e = 1;
            d++;

            while (true)
            {
                while (true)
                {
                    f = b * e;
                    d++;
                    f = c == f ? 1 : 0;
                    d++;
                    d += f;
                    d++;

                    if (f == 0)
                    {
                        d++;
                    }
                    else
                    {
                        a += b;
                        PuzzleConsole.WriteLine(b);
                    }

                    d++;
                    e++;

                    d++;
                    f = c == e ? 1 : 0;
                    d++;
                    d += f;
                    d++;

                    if (d > 11)
                    {
                        break;
                    }

                    d = 3;
                }

                b += 1;
                d++;
                f = b > c ? 1 : 0;
                d++;
                d += f;
                d++;

                if (f == 1)
                {
                    break;
                }

                d = 3;
                e = 1;
            }

            return a;
        }

        public int Run()
        {
            this.Registers[0] = 0;

            while (this.InstructionPointer < this.Program.Count)
            {
                Instruction instruction = this.Program[this.InstructionPointer];

                this.Registers[this.InstructionRegister] = this.InstructionPointer;

                this.OpCodes[instruction.Type](instruction.A, instruction.B, instruction.C);

                this.InstructionPointer = this.Registers[this.InstructionRegister] + 1;
            }

            return this.Registers[0];
        }

        private static int GenerateNumber()
        {
            int a = 1;
            int c = 0;
            int d = 0;
            int f = 0;

            d += 16;
            c += 2;
            c *= c;
            d += 3;
            c *= d;
            c *= 11;
            f += 3;
            d += 3;
            f *= d;
            f += 3;
            c += f;
            d += a;
            d += 4;
            f = d;
            d++;
            f *= d;
            d++;
            f += d;
            d++;
            f *= d;
            d++;
            f *= 14;
            d++;
            f *= d;
            c += f;

            return c;
        }

        private void Addr(int a, int b, int c) => this.Registers[c] = this.Registers[a] + this.Registers[b];

        private void Addi(int a, int b, int c) => this.Registers[c] = this.Registers[a] + b;

        private void Mulr(int a, int b, int c) => this.Registers[c] = this.Registers[a] * this.Registers[b];

        private void Muli(int a, int b, int c) => this.Registers[c] = this.Registers[a] * b;

        private void Banr(int a, int b, int c) => this.Registers[c] = this.Registers[a] & this.Registers[b];

        private void Bani(int a, int b, int c) => this.Registers[c] = this.Registers[a] & b;

        private void Borr(int a, int b, int c) => this.Registers[c] = this.Registers[a] | this.Registers[b];

        private void Bori(int a, int b, int c) => this.Registers[c] = this.Registers[a] | b;

        private void Setr(int a, int b, int c) => this.Registers[c] = this.Registers[a];

        private void Seti(int a, int b, int c) => this.Registers[c] = a;

        private void Gtir(int a, int b, int c) => this.Registers[c] = a > this.Registers[b] ? 1 : 0;

        private void Gtri(int a, int b, int c) => this.Registers[c] = this.Registers[a] > b ? 1 : 0;

        private void Gtrr(int a, int b, int c) => this.Registers[c] = this.Registers[a] > this.Registers[b] ? 1 : 0;

        private void Eqir(int a, int b, int c) => this.Registers[c] = a == this.Registers[b] ? 1 : 0;

        private void Eqri(int a, int b, int c) => this.Registers[c] = this.Registers[a] == b ? 1 : 0;

        private void Eqrr(int a, int b, int c) => this.Registers[c] = this.Registers[a] == this.Registers[b] ? 1 : 0;
    }
}
