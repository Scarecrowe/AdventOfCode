namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_07___No_Space_Left_On_Device;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
        {
            this.DayTitle = "No Space Left On Device";
            this.GetPuzzleData(7, this.DayTitle);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new NoSpaceLeftOnDevice(this.Input).Sum()}";

        public string Gold() => $"{new NoSpaceLeftOnDevice(this.Input).Min()}";
    }
}
