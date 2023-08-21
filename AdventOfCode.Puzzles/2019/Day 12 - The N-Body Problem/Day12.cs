namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_12___The_N_Body_Problem;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12(string file)
        {
            this.DayTitle = "The N-Body Problem";
            this.GetPuzzleData(file);
        }

        public Day12(string[] input) => this.Input = input;

        public string Silver() => $"{new TheNBodyProblem(this.Input).Simulate(1000)}";

        public string Gold() => $"{new TheNBodyProblem(this.Input).Simulate()}";
    }
}
