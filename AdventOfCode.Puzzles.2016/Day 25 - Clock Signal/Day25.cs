namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_25___Clock_Signal;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25()
        {
            this.DayTitle = "Clock Signal";
            this.GetPuzzleData(25, this.DayTitle);
        }

        public Day25(string[] input) => this.Input = input;

        public string Silver() => $"{new ClockSignal(this.Input).LowesetPositiveInt()}";

        public string Gold() => $"You have enough stars to [Transmit the Signal]";
    }
}
