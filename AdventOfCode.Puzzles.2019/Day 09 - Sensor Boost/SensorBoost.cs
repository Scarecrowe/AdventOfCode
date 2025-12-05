namespace AdventOfCode.Puzzles._2019.Day_09___Sensor_Boost
{
    using AdventOfCode.Puzzles._2019.IntCode;

    public class SensorBoost
    {
        public SensorBoost(string input) => this.Cpu = new IntcodeCpu(input);

        public IntcodeCpu Cpu { get; }

        public long RunTestMode()
        {
            this.Cpu.Input.Enqueue(1);
            this.Cpu.Run();

            return this.Cpu.Output.Dequeue();
        }

        public long RunBoost()
        {
            this.Cpu.Input.Enqueue(2);
            this.Cpu.Run();

            return this.Cpu.Output.Dequeue();
        }
    }
}