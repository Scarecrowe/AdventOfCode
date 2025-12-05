namespace AdventOfCode.Puzzles._2019.Day_02___1202_Program_Alarm
{
    using AdventOfCode.Puzzles._2019.IntCode;

    public class OneTwoOneTwoProgramAlarm
    {
        public OneTwoOneTwoProgramAlarm(string input) => this.Cpu = new IntcodeCpu(input);

        public IntcodeCpu Cpu { get; }

        public long Run(int noun = 0, int verb = 0)
        {
            int minNoun = noun == 0 ? 0 : noun;
            int minVerb = verb == 0 ? 0 : verb;
            int maxNoun = noun == 0 ? 99 : noun;
            int maxVerb = verb == 0 ? 99 : verb;

            for (int i = minNoun; i <= maxNoun; i++)
            {
                for (int j = minVerb; j <= maxVerb; j++)
                {
                    this.Cpu.Memory.Write(1, i);
                    this.Cpu.Memory.Write(2, j);
                    this.Cpu.Run();

                    if (noun == 0 && this.Cpu.Memory.Read(0) == 19690720)
                    {
                        return (100 * i) + j;
                    }

                    if (noun == 0)
                    {
                        this.Cpu.Reset();
                    }
                }
            }

            return this.Cpu.Memory.Read(0);
        }
    }
}
