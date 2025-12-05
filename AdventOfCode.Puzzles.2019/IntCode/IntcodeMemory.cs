namespace AdventOfCode.Puzzles._2019.IntCode
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class IntcodeMemory
    {
        public IntcodeMemory(string input)
            => this.RAM = $"{input}{",0".Repeat(5000)}".Split(",").ToLong();

        public IntcodeMemory(long[] input) => this.RAM = (long[])input.Clone();

        public long[] RAM { get; private set; }

        public long Read(long address) => this.RAM[address];

        public void Write(long address, long value) => this.RAM[address] = value;

        public IntcodeMemory Print()
        {
            for (int i = 0; i < this.RAM.Length; i++)
            {
                IntcodeOpcode opcode = new((IntcodeOpcodeType)this.RAM[i]);

                PuzzleConsole.Write($"{i,-4}{opcode.ToString()}");

                for (int j = i + 1; j <= i + opcode.ParameterCount; j++)
                {
                    PuzzleConsole.Write($" {this.RAM[j]}");
                }

                PuzzleConsole.WriteLine();

                i += opcode.ParameterCount;
            }

            return this;
        }
    }
}
