namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_06___Signals_and_Noise;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6()
            : base(2016, 6, "Signals and Noise")
        {
        }

        public Day6(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{SignalsAndNoise.ValidMessage(this.Input)}";

        public string Gold() => $"{SignalsAndNoise.ValidMessage(this.Input, false)}";
    }
}
