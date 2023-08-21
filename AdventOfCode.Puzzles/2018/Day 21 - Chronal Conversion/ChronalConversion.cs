namespace AdventOfCode.Puzzles._2018.Day_21___Chronal_Conversion
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ChronalConversion
    {
        public ChronalConversion(string[] input)
        {
            this.Program = new List<Instruction>();

            foreach (string line in input.Skip(1))
            {
                this.Program.Add(new(line));
            }

            this.Registers = new int[6];

            this.InstructionRegister = input[0].Replace("#ip ").ToInt();
        }

        public List<Instruction> Program { get; }

        public int InstructionRegister { get; }

        public int[] Registers { get; }

        public int LowestNonNegative() => this.ValidRegisterA().Take(1).Select(c => c.c).First();

        public int MostNonNegative() => this.ValidRegisterA().Last().c;

        public ChronalConversion PrintProgram()
        {
            PuzzleConsole.WriteLine("Program Instructions");

            foreach (Instruction instruction in this.Program)
            {
                PuzzleConsole.WriteLine($"{instruction.Print(this, true)}");
            }

            PuzzleConsole.WriteLine();
            return this;
        }

        private IEnumerable<(int c, int count)> ValidRegisterA()
        {
            Dictionary<int, int> results = new();
            int c = 0; // 5
            int count = 5;
            int f;
        TryOverflow:
            f = c | 65536; // 6
            c = 5234604; // 7

            count += 2;
            int d;
        CalculateC:
            d = f & 255; // 8
            c += d; // 9
            c &= 16777215; // 10
            c *= 65899; // 11
            c &= 16777215; // 12
            d = f < 256 ? 1 : 0; // 13

            count += 8;

            if (d == 1)
            {
                goto ResultCheck; // GOTO 28
            }

            d = 0; // 17

            int b;
        Loop:
            b = d + 1; // 18
            b *= 256; // 19
            b = b > f ? 1 : 0; // 20

            count += 5;

            if (b == 1)
            {
                goto LoopFinish; // GOTO 26
            }

            d += 1; // 24

            goto Loop; // GOTO 18

        LoopFinish: // 26
            f = d;

            count += 2;
            goto CalculateC; // GOTO 8;

        ResultCheck: // 28
            if (!results.ContainsKey(c))
            {
                results.Add(c, count);
                yield return (c, count);

                goto TryOverflow; // GOTO 6
            }
        }
    }
}
