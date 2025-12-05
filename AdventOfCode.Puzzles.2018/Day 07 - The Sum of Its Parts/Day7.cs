namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_07___The_Sum_of_Its_Parts;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
        {
            this.DayTitle = "The Sum of Its Parts";
            this.GetPuzzleData(7, this.DayTitle);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new TheSumOfItsParts(this.Input).AssembleyOrder()}";

        public string Gold() => $"{new TheSumOfItsParts(this.Input).AssemblyTime(5)}";
    }
}
