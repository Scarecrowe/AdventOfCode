namespace AdventOfCode.Puzzles._2019.Day_21___Springdroid_Adventure
{
    using AdventOfCode.Puzzles._2019.IntCode;

    public class SpringdroidAdventure
    {
        public SpringdroidAdventure(string program) => this.Cpu = new IntcodeCpu(program);

        private IntcodeCpu Cpu { get; }

        public long Run()
        {
            this.Cpu.RunAsciiCommand();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT C J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("AND D J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT A T"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("OR T J"));
            this.Cpu.RunAsciiCommand("WALK");

            return this.Cpu.Output.FirstOrDefault(c => c > 255);
        }

        public long RunWihtInreasedSensor()
        {
            this.Cpu.RunAsciiCommand();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT C J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("AND D J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("AND H J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT B T"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("AND D T"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("OR T J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT A T"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("OR T J"));
            this.Cpu.RunAsciiCommand("RUN");

            return this.Cpu.Output.FirstOrDefault(c => c > 255);
        }
    }
}
