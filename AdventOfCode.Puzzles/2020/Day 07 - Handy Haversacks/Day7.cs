namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_07___Handy_Haversacks;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7(string file)
        {
            this.DayTitle = "Handy Haversacks";
            this.GetPuzzleData(file);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new HandyHaversacks(this.Input).ShinyGoldCount()}";

        public string Gold() => $"{new HandyHaversacks(this.Input).RequiredBags()}";
    }
}
