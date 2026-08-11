namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_10___Hoof_It;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10()
            : base(2024, 10, "Hoof It")
        {
        }

        public Day10(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new HoofIt(this.Input).TrailHeads(false)}";

        public string Gold() => $"{new HoofIt(this.Input).TrailHeads(true)}";
    }
}
