namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_15___Chiton;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15(string file)
        {
            this.DayTitle = "Chiton";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day15(string[] input) => this.Input = input;

        public string Silver() => $"{new ChitonNavigator(this.Input).Navigate()}";

        public string Gold() => $"{new ChitonNavigator(this.Input).Enlarge().Navigate()}";
    }
}
