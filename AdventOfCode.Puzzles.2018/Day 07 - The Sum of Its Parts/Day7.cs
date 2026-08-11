namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_07___The_Sum_of_Its_Parts;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
            : base(2018, 7, "The Sum of Its Parts")
        {
        }

        public Day7(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TheSumOfItsParts(this.Input).AssembleyOrder()}";

        public string Gold() => $"{new TheSumOfItsParts(this.Input).AssemblyTime(5)}";
    }
}
