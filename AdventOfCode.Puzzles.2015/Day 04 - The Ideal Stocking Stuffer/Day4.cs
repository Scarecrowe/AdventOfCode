namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_04___The_Ideal_Stocking_Stuffer;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
        {
            this.DayTitle = "The Ideal Stocking Stuffer";
            this.GetPuzzleData(4, this.DayTitle, StringSplitOptions.None);
        }

        public Day4(string[] input) => this.Input = input;

        public string Silver() => $"{new TheIdealStockingStuffer(this.Input[0], 5).Number}";

        public string Gold() => $"{new TheIdealStockingStuffer(this.Input[0], 6).Number}";
    }
}
