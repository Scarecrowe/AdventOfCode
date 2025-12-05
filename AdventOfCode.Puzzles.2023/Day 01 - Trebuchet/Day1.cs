namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_01___Trebuchet;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
        {
            this.DayTitle = "Trebuchet?!";
            this.GetPuzzleData(1, this.DayTitle);
        }

        public string Silver() => $"{new Trebuchet(this.Input, false).Sum()}";

        public string Gold() => $"{new Trebuchet(this.Input).Sum()}";
    }
}
