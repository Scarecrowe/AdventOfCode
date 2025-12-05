namespace AdventOfCode.Puzzles._2019.Day_25___Cryostasis
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class Cryostasis
    {
        public Cryostasis(string program) => this.Cpu = new(program);

        public IntcodeCpu Cpu { get; }

        public string Run()
        {
            this.Cpu.RunAsciiCommand();
            this.Cpu.RunAsciiCommand("north");
            this.Cpu.RunAsciiCommand("north");
            this.Cpu.RunAsciiCommand("north");
            this.Cpu.RunAsciiCommand("take mutex");
            this.Cpu.RunAsciiCommand("south");
            this.Cpu.RunAsciiCommand("south");
            this.Cpu.RunAsciiCommand("east");
            this.Cpu.RunAsciiCommand("north");
            this.Cpu.RunAsciiCommand("take loom");
            this.Cpu.RunAsciiCommand("south");
            this.Cpu.RunAsciiCommand("west");
            this.Cpu.RunAsciiCommand("south");
            this.Cpu.RunAsciiCommand("west");
            this.Cpu.RunAsciiCommand("west");
            this.Cpu.RunAsciiCommand("take sand");
            this.Cpu.RunAsciiCommand("south");
            this.Cpu.RunAsciiCommand("east");
            this.Cpu.RunAsciiCommand("north");
            this.Cpu.RunAsciiCommand("take wreath");
            this.Cpu.RunAsciiCommand("south");
            this.Cpu.RunAsciiCommand("west");
            this.Cpu.RunAsciiCommand("north");
            this.Cpu.RunAsciiCommand("north");
            this.Cpu.RunAsciiCommand("east");

            this.Cpu.Output.Clear();

            this.Cpu.RunAsciiCommand("east");

            return this.Cpu.Output.Select(c => (char)c).ToArray().Join().Numbers();
        }

        public string Play()
        {
            this.Cpu.Run();

            while (this.Cpu.State != IntcodeCpuState.Terminated)
            {
                PuzzleConsole.Write(this.Cpu.Output.Select(c => (char)c).ToArray().Join());
                PuzzleConsole.Flush();
                this.Cpu.Output.Clear();

                this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii(Console.ReadLine() ?? string.Empty));
                this.Cpu.Run();
            }

            PuzzleConsole.Write(this.Cpu.Output.Select(c => (char)c).ToArray().Join());
            PuzzleConsole.Flush();

            return string.Empty;
        }
    }
}
