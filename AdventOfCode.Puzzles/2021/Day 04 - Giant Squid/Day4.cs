namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_04___Giant_Squid;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4(string file)
        {
            this.DayTitle = "Giant Squid";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day4(string[] input) => this.Input = input;

        public string Silver() => $"{GiantSquid.First(this.Input)}";

        public string Gold() => $"{GiantSquid.Last(this.Input)}";
    }
}
