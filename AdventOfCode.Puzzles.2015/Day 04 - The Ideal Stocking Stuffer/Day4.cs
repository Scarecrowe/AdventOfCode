namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_04___The_Ideal_Stocking_Stuffer;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
            : base(2015, 4, "The Ideal Stocking Stuffer", StringSplitOptions.None)
        {
        }

        public Day4(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TheIdealStockingStuffer(this.Input[0], 5).Number}";

        public string Gold() => $"{new TheIdealStockingStuffer(this.Input[0], 6).Number}";
    }
}
