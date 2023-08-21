namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_18___Duet;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18(string file)
        {
            this.DayTitle = "Duet";
            this.GetPuzzleData(file);
        }

        public Day18(string[] input) => this.Input = input;

        public string Silver() => $"{new Duet(this.Input, 0).Process(null)}";

        [Slow]
        public string Gold() => $"{new Motherboard(this.Input).Run()}";
    }
}
