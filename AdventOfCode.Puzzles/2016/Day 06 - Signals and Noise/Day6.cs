namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_06___Signals_and_Noise;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6(string file)
        {
            this.DayTitle = "Signals and Noise";
            this.GetPuzzleData(file);
        }

        public Day6(string[] input) => this.Input = input;

        public string Silver() => $"{SignalsAndNoise.ValidMessage(this.Input)}";

        public string Gold() => $"{SignalsAndNoise.ValidMessage(this.Input, false)}";
    }
}
