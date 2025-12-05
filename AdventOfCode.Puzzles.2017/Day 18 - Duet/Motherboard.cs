namespace AdventOfCode.Puzzles._2017.Day_18___Duet
{
    public class Motherboard
    {
        public Motherboard(string[] input)
        {
            this.CpuA = new(input, 0);
            this.CpuB = new(input, 1);
        }

        public Duet CpuA { get; }

        public Duet CpuB { get; }

        public long Run()
        {
            long result = 0;

            Thread threadA = new(() =>
            {
                this.CpuA.Process(this.CpuB);
            });

            threadA.Start();

            Thread threadB = new(() =>
            {
                result = this.CpuB.Process(this.CpuA);
            });

            threadB.Start();
            threadA.Join();
            threadB.Join();

            return result;
        }
    }
}
