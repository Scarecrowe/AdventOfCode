namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_03___Squares_With_Three_Sides;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
            : base(2016, 3, "Squares With Three Sides")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{SquaresWithThreeSides.Valid(this.Input)}";

        public string Gold() => $"{SquaresWithThreeSides.ValidByColumns(this.Input)}";
    }
}
