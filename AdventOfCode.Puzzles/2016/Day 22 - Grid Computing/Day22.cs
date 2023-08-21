namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_22___Grid_Computing;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22(string file)
        {
            this.DayTitle = "Grid Computing";
            this.GetPuzzleData(file);
        }

        public Day22(string[] input) => this.Input = input;

        public string Silver() => $"{new GridComputing(this.Input).FindAvailablePairs()}";

        public string Gold() => $"{new GridComputing(this.Input).MoveData()}";
    }
}
