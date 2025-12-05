namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_06___Lanternfish;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6()
        {
            this.DayTitle = "Lanternfish";
            this.GetPuzzleData(6, this.DayTitle);
        }

        public Day6(string[] input) => this.Input = input;

        public string Silver() => $"{new LanternFishSpawner(this.Input).Run(80).TotalFish()}";

        public string Gold() => $"{new LanternFishSpawner(this.Input).Run(256).TotalFish()}";
    }
}
