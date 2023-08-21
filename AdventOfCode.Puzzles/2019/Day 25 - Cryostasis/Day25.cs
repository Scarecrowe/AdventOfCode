namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_25___Cryostasis;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25(string file)
        {
            this.DayTitle = "Category Six";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day25(string[] input) => this.Input = input;

        public string Silver() => $"{new Cryostasis(this.Input[0]).Run()}";

        public string Gold() => $"You have enough stars to [Align the Warp Drive]";
    }
}
