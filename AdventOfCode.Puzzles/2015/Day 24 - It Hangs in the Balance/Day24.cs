namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_24___It_Hangs_in_the_Balance;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24(string file)
        {
            this.DayTitle = "It Hangs in the Balance";
            this.GetPuzzleData(file);
        }

        public Day24(string[] input) => this.Input = input;

        public string Silver() => $"{new ItHangsInTheBalance(this.Input).IdealConfiguration(3)}";

        public string Gold() => $"{new ItHangsInTheBalance(this.Input).IdealConfiguration(4)}";
    }
}
