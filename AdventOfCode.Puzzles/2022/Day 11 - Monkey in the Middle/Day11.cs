namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_11___Monkey_in_the_Middle;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11(string file)
        {
            this.DayTitle = "Monkey in the Middle";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{new MonkeyInTheMiddle(this.Input.ToList()).Play(20)}";

        public string Gold() => $"{new MonkeyInTheMiddle(this.Input.ToList()).Play(10000)}";
    }
}
