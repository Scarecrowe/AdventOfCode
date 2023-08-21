namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_03___No_Matter_How_You_Slice_It;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3(string file)
        {
            this.DayTitle = "No Matter How You Slice It";
            this.GetPuzzleData(file);
        }

        public Day3(string[] input) => this.Input = input;

        public string Silver() => $"{new NoMatterHowYouSliceIt(this.Input).PlotClaims().OverlappingClaims()}";

        public string Gold() => $"{new NoMatterHowYouSliceIt(this.Input).PlotClaims().NonOverlappingClaim()}";
    }
}
