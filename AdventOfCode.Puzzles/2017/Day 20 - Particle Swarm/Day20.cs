namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_20___Particle_Swarm;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20(string file)
        {
            this.DayTitle = "Particle Swarm";
            this.GetPuzzleData(file);
        }

        public Day20(string[] input) => this.Input = input;

        public string Silver() => $"{new ParticleSwam(this.Input).Run()}";

        [Slow]
        public string Gold() => $"{new ParticleSwam(this.Input).Run(true)}";
    }
}
