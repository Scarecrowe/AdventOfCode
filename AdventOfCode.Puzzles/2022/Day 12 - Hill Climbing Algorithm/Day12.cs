namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_12___Hill_Climbing_Algorithm;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12(string file)
        {
            this.DayTitle = "Hill Climbing Algorithm";
            this.GetPuzzleData(file);
        }

        public Day12(string[] input) => this.Input = input;

        public string Silver() => $"{new HillClimbingAlgorithm(this.Input).Fewest()}";

        public string Gold() => $"{new HillClimbingAlgorithm(this.Input, false).Fewest()}";
    }
}
