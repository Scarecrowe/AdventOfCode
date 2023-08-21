namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_10___Monitoring_Station;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10(string file)
        {
            this.DayTitle = "Monitoring Station";
            this.GetPuzzleData(file);
        }

        public Day10(string[] input) => this.Input = input;

        [Slow]
        public string Silver() => $"{new MonitoringStation(this.Input).FindBestLocation().MaxTargets}";

        [Slow]
        public string Gold() => $"{new MonitoringStation(this.Input).FindBestLocation().ClearAsteroidField().VaporizedScore()}";
    }
}
