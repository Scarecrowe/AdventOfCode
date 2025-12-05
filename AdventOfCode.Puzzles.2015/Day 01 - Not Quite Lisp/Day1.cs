namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_01___Not_Quite_Lisp;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
        {
            this.DayTitle = "Not Quite Lisp";
            this.GetPuzzleData(1, this.DayTitle);
        }

        public Day1(string[] input) => this.Input = input;

        public string Silver() => $"{new NotQuiteLisp(this.Input).FinalPosition()}";

        public string Gold() => $"{new NotQuiteLisp(this.Input).BasementPosition()}";
    }
}
