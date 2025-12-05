namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_07___The_Treachery_of_Whales;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
        {
            this.DayTitle = "The Treachery of Whales";
            this.GetPuzzleData(7, this.DayTitle);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new TheTreacheryOfWhales(this.Input).Calculate(true)}";

        public string Gold() => $"{new TheTreacheryOfWhales(this.Input).Calculate(false)}";
    }
}
