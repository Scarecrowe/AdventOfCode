namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_24___Air_Duct_Spelunking;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24()
        {
            this.DayTitle = "Air Duct Spelunking";
            this.GetPuzzleData(24, this.DayTitle);
        }

        public Day24(string[] input) => this.Input = input;

        public string Silver() => $"{new AirDuctSpelunking(this.Input).ShortestPath()}";

        public string Gold() => $"{new AirDuctSpelunking(this.Input).ShortestPath(true)}";
    }
}
