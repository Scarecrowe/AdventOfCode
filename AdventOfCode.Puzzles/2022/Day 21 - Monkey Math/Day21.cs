namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_21___Monkey_Math;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21(string file)
        {
            this.DayTitle = "Monkey Math";
            this.GetPuzzleData(file);
        }

        public Day21(string[] input) => this.Input = input;

        public string Silver() => $"{new MonkeyMath(this.Input).Root()}";

        public string Gold() => $"{new MonkeyMath(this.Input).RootEqual()}";
    }
}
