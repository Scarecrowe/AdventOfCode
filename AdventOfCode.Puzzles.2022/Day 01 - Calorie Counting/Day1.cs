namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_01___Calorie_Counting;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
        {
            this.DayTitle = "Calorie Counting";
            this.GetPuzzleData(1, this.DayTitle, StringSplitOptions.None);
        }

        public Day1(string[] input) => this.Input = input;

        public string Silver() => $"{new CalorieCounting(this.Input).MaxCallories()}";

        public string Gold() => $"{new CalorieCounting(this.Input).TopThreeMaxCallories()}";
    }
}
