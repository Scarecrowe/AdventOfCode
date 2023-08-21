namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_10___Balance_Bots;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10(string file)
        {
            this.DayTitle = "Balance Bots";
            this.GetPuzzleData(file);
        }

        public Day10(string[] input) => this.Input = input;

        public string Silver() => $"{new BalanceBot(this.Input).Process().ComparerBot(17, 61)}";

        public string Gold() => $"{new BalanceBot(this.Input).Process().MultipleOutputs()}";
    }
}
