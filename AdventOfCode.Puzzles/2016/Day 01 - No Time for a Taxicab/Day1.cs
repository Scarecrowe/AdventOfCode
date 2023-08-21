namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_01___No_Time_for_a_Taxicab;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1(string file)
        {
            this.DayTitle = "No Time for a Taxicab";
            this.GetPuzzleData(file);
        }

        public Day1(string[] input) => this.Input = input;

        public string Silver() => $"{new NoTimeForATaxicab(this.Input).Travel()}";

        public string Gold() => $"{new NoTimeForATaxicab(this.Input).Travel(true)}";
    }
}
