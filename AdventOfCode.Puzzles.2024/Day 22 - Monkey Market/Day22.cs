namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_22___Monkey_Market;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
        {
            this.DayTitle = "Monkey Market";
            this.GetPuzzleData(22, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new MonkeyMarket(this.Input).Sum()}";

        public string Gold() => $"{new MonkeyMarket(this.Input).Best()}";
    }
}
