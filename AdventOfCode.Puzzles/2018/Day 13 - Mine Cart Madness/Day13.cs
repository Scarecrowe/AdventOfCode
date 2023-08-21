namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_13___Mine_Cart_Madness;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13(string file)
        {
            this.DayTitle = "Mine Cart Madness";
            this.GetPuzzleData(file);
        }

        public Day13(string[] input) => this.Input = input;

        public string Silver() => $"{new MineCartMadness(this.Input).Tick()}";

        public string Gold() => $"{new MineCartMadness(this.Input).Tick(false)}";
    }
}
