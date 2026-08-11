namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_01___Historian_Hysteria;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
            : base(2024, 1, "Historian Hysteria")
        {
        }

        public Day1(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new HistorianHysteria(this.Input).Distance()}";

        public string Gold() => $"{new HistorianHysteria(this.Input).Similarity()}";
    }
}
