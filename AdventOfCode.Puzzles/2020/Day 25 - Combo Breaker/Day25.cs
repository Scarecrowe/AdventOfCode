namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_25___Combo_Breaker;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25(string file)
        {
            this.DayTitle = "Combo Breaker";
            this.GetPuzzleData(file);
        }

        public Day25(string[] input) => this.Input = input;

        public string Silver() => $"{ComboBreaker.EncryptiongKey(this.Input)}";

        public string Gold() => $"You have enough stars to [Check On Your Deposit]";
    }
}
