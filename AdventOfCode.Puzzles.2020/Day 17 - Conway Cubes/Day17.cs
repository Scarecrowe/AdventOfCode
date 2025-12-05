namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_17___Conway_Cubes;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17()
        {
            this.DayTitle = "Conway Cubes";
            this.GetPuzzleData(17, this.DayTitle, StringSplitOptions.None);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{ConwayCubes.Simple(this.Input)}";

        public string Gold() => $"{ConwayCubes.Advanced(this.Input)}";
    }
}
