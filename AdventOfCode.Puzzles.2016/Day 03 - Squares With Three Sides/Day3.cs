namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_03___Squares_With_Three_Sides;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
        {
            this.DayTitle = "Squares With Three Sides";
            this.GetPuzzleData(3, this.DayTitle);
        }

        public Day3(string[] input) => this.Input = input;

        public string Silver() => $"{SquaresWithThreeSides.Valid(this.Input)}";

        public string Gold() => $"{SquaresWithThreeSides.ValidByColumns(this.Input)}";
    }
}
