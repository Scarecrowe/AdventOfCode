namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_12___The_N_Body_Problem;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12()
            : base(2019, 12, "The N-Body Problem")
        {
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TheNBodyProblem(this.Input).Simulate(1000)}";

        public string Gold() => $"{new TheNBodyProblem(this.Input).Simulate()}";
    }
}
