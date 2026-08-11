namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_20___Pulse_Propagation;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20()
            : base(2023, 20, "Pulse Propagation", StringSplitOptions.None)
        {
        }

        public Day20(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new PulsePropagation(this.Input).LowAndHighMultiplied()}";

        public string Gold() => $"{new PulsePropagation(this.Input).FewestPresses()}";
    }
}
