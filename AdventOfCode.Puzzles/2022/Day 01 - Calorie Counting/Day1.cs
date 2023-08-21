namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_01___Calorie_Counting;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1(string file)
        {
            this.DayTitle = "Calorie Counting";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day1(string[] input) => this.Input = input;

        public string Silver() => $"{new CalorieCounting(this.Input).MaxCallories()}";

        public string Gold() => $"{new CalorieCounting(this.Input).TopThreeMaxCallories()}";
    }
}
