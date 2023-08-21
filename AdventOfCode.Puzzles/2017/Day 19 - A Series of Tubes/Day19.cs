namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_19___A_Series_of_Tubes;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19(string file)
        {
            this.DayTitle = "A Series of Tubes";
            this.GetPuzzleData(file);
        }

        public Day19(string[] input) => this.Input = input;

        public string Silver() => $"{new ASeriesOfTubes(this.Input).Move()}";

        public string Gold() => $"{new ASeriesOfTubes(this.Input).Move(true)}";
    }
}
