namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_17___Two_Steps_Forward;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17(string file)
        {
            this.DayTitle = "Two Steps Forward";
            this.GetPuzzleData(file);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{new TwoStepsForward().AccessVault(this.Input[0]).ShortestPath()}";

        public string Gold() => $"{new TwoStepsForward().AccessVault(this.Input[0]).LongestPath()}";
    }
}
