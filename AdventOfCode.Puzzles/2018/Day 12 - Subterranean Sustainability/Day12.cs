namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_12___Subterranean_Sustainability;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12(string file)
        {
            this.DayTitle = "Subterranean Sustainability";
            this.GetPuzzleData(file);
        }

        public Day12(string[] input) => this.Input = input;

        public string Silver() => $"{new SubterraneanSustainability(this.Input).Grow(20)}";

        public string Gold() => $"{new SubterraneanSustainability(this.Input).Grow(50000000000)}";
    }
}
