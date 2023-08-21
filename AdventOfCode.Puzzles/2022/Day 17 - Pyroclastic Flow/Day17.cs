namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_17___Pyroclastic_Flow;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17(string file)
        {
            this.DayTitle = "Pyroclastic Flow";
            this.GetPuzzleData(file);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{new PyroclasticFlow(this.Input).Stack(2022)}";

        public string Gold() => $"{new PyroclasticFlow(this.Input).Stack(1000000000000)}";
    }
}
