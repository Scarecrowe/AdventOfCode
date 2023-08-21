namespace AdventOfCode.Puzzles._2020.Day_08___Handheld_Halting
{
    public class HandheldHalting
    {
        public HandheldHalting(string[] program)
        {
            this.Program = program;
            this.Stack = new();
        }

        public int Accumulator { get; private set; }

        public List<int> Stack { get; }

        public bool InfiniteLoop { get; private set; }

        private string[] Program { get; }

        public HandheldHalting Execute()
        {
            for (int i = 0; i < this.Program.Length; i++)
            {
                if (this.Stack.Contains(i))
                {
                    this.InfiniteLoop = true;
                    break;
                }

                this.Stack.Add(i);

                Instruction instruction = new(this.Program[i]);

                switch (instruction.Code)
                {
                    case "nop":
                        break;
                    case "acc":
                        this.Accumulator = instruction.Plus
                            ? this.Accumulator + instruction.Value
                            : this.Accumulator - instruction.Value;
                        break;
                    case "jmp":
                        i = (instruction.Plus
                            ? i + instruction.Value
                            : i - instruction.Value) - 1;
                        break;
                }
            }

            return this;
        }

        public int Fixed()
        {
            for (int x = 0; x < this.Program.Length; x++)
            {
                Instruction current = new(this.Program[x]);

                if (current.IsNopOrJump)
                {
                    this.Program[x] = current.ReverseNopOrJmp();
                }

                HandheldHalting cpu = new HandheldHalting(this.Program).Execute();

                if (!cpu.InfiniteLoop)
                {
                    return cpu.Accumulator;
                }

                this.Program[x] = current.Raw;
            }

            return -1;
        }
    }
}
