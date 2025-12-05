namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_10___The_Stars_Align;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10()
        {
            this.DayTitle = "The Stars Align";
            this.GetPuzzleData(10, this.DayTitle);
        }

        public Day10(string[] input) => this.Input = input;

        public string Silver() => $"{new TheStarsAlign(this.Input).Align()}";

        public string Gold() => $"{new TheStarsAlign(this.Input).Align(false)}";
    }
}
