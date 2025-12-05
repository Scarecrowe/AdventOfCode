namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_22___Monkey_Map;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
        {
            this.DayTitle = "Monkey Map";
            this.GetPuzzleData(22, this.DayTitle, StringSplitOptions.None);
        }

        public Day22(string[] input) => this.Input = input;

        public string Silver() => $"{new MonkeyMap(this.Input).Navigate(MonkeyMapType.TwoDimensional).Password()}";

        public string Gold() => $"{new MonkeyMap(this.Input).Navigate(MonkeyMapType.ThreeDimensional).Password()}";
    }
}
