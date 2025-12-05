namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_23___Safe_Cracking;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23()
        {
            this.DayTitle = "Safe Cracking";
            this.GetPuzzleData(23, this.DayTitle);
        }

        public Day23(string[] input) => this.Input = input;

        public string Silver() => $"{new SafeCracking(this.Input.Take(26).ToArray()).Simple()}";

        public string Gold() => $"{new SafeCracking(this.Input.Skip(26).ToArray()).Advanced()}";
    }
}
