namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_04___Security_Through_Obscurity;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
            : base(2016, 4, "Security Through Obscurity")
        {
        }

        public Day4(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{SecurityThroughObscurity.SumOfValidSectors(this.Input)}";

        public string Gold() => $"{SecurityThroughObscurity.NorthPoleSectorId(this.Input)}";
    }
}
