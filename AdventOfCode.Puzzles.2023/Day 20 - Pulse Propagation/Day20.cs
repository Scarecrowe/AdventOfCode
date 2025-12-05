namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_20___Pulse_Propagation;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20()
        {
            this.DayTitle = "Pulse Propagation";
            this.GetPuzzleData(20, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new PulsePropagation(this.Input).LowAndHighMultiplied()}";

        public string Gold() => $"{new PulsePropagation(this.Input).FewestPresses()}";
    }
}
