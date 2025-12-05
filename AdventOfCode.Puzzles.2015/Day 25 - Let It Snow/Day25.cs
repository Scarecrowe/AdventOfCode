namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_25___Let_It_Snow;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25()
        {
            this.DayTitle = "Let It Snow";
            this.GetPuzzleData(25, this.DayTitle, StringSplitOptions.None);
        }

        public Day25(string[] input) => this.Input = input;

        public string Silver() => $"{new LetItSnow(this.Input).Generate()}";

        public string Gold() => $"You have enough stars to [Turn It Off and On]";
    }
}
