namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_12___Rain_Risk;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12(string file)
        {
            this.DayTitle = "Rain Risk";
            this.GetPuzzleData(file);
        }

        public Day12(string[] input) => this.Input = input;

        public string Silver() => $"{new RainRisk(this.Input).Distance()}";

        public string Gold() => $"{new RainRisk(this.Input).DistanceWithWaypoint()}";
    }
}
