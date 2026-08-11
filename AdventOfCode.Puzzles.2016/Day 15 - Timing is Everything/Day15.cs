namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_15___Timing_is_Everything;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15()
            : base(2016, 15, "Timing is Everything")
        {
        }

        public Day15(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TimingIsEverything(this.Input).SimulateBall()}";

        public string Gold() => $"{new TimingIsEverything(this.Input, true).SimulateBall()}";
    }
}
