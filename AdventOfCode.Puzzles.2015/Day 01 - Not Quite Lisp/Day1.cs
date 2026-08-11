namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_01___Not_Quite_Lisp;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
            : base(2015, 1, "Not Quite Lisp")
        {
        }

        public Day1(string[] input)
         : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new NotQuiteLisp(this.Input).FinalPosition()}";

        public string Gold() => $"{new NotQuiteLisp(this.Input).BasementPosition()}";
    }
}
