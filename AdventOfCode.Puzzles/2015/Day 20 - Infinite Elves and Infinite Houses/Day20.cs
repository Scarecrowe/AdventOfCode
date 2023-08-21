namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_20___Infinite_Elves_and_Infinite_Houses;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20(string file)
        {
            this.DayTitle = "Infinite Elves and Infinite Houses";
            this.GetPuzzleData(file);
        }

        public Day20(string[] input) => this.Input = input;

        public string Silver() => $"{new InfiniteElvesAndInfiniteHouses(this.Input).Lowest(false)}";

        public string Gold() => $"{new InfiniteElvesAndInfiniteHouses(this.Input).Lowest(true)}";
    }
}
