namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_01___Calorie_Counting;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
            : base(2022, 1, "Calorie Counting", StringSplitOptions.None)
        {
        }

        public Day1(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CalorieCounting(this.Input).MaxCallories()}";

        public string Gold() => $"{new CalorieCounting(this.Input).TopThreeMaxCallories()}";
    }
}
