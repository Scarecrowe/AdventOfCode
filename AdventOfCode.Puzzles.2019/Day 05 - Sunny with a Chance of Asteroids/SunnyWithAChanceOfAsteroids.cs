namespace AdventOfCode.Puzzles._2019.Day_05___Sunny_with_a_Chance_of_Asteroids
{
    using AdventOfCode.Puzzles._2019.IntCode;

    public class SunnyWithAChanceOfAsteroids
    {
        public SunnyWithAChanceOfAsteroids(string input) => this.Cpu = new IntcodeCpu(input);

        public IntcodeCpu Cpu { get; }

        public long AirConditionerDiagnostics()
        {
            this.Cpu.Input.Enqueue(1);
            this.Cpu.Run();

            return this.Cpu.Output.Last();
        }

        public long ThermalRadiatorDiagnostics()
        {
            this.Cpu.Input.Enqueue(5);
            this.Cpu.Run();

            return this.Cpu.Output.Dequeue();
        }
    }
}
