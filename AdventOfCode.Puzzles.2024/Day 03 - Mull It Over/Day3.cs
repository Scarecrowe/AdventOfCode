namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_03___Mull_It_Over;

    public class Day3 : Puzzle, IPuzzle
    {

        public Day3()
            : base(2024, 3, "Mull It Over")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new MullItOver(this.Input, false).Calculate()}";

        public string Gold() => $"{new MullItOver(this.Input, true).Calculate()}";
    }
}
