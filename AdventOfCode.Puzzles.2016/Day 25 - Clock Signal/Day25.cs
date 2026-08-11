namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_25___Clock_Signal;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25()
            : base(2016, 25, "Clock Signal")
        {
        }

        public Day25(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ClockSignal(this.Input).LowesetPositiveInt()}";

        public string Gold() => $"You have enough stars to [Transmit the Signal]";
    }
}
