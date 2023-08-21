namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_23___Crab_Cups;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23(string file)
        {
            this.DayTitle = "Crab Cups";
            this.GetPuzzleData(file);
        }

        public Day23(string[] input) => this.Input = input;

        public string Silver() => $"{CrabCups.Short(this.Input)}";

        public string Gold() => $"{CrabCups.Long(this.Input)}";
    }
}
