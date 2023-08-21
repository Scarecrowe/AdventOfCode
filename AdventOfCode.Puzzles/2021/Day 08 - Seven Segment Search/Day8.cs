namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_08___Seven_Segment_Search;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8(string file)
        {
            this.DayTitle = "Seven Segment Search";
            this.GetPuzzleData(file);
        }

        public Day8(string[] input) => this.Input = input;

        public string Silver() => $"{new SevenSegmentDisplay(this.Input).UniqueOutputValues()}";

        public string Gold() => $"{new SevenSegmentDisplay(this.Input).DisplayValue()}";
    }
}
