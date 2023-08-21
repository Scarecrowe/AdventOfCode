namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_20___Trench_Map;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20(string file)
        {
            this.DayTitle = "Trench Map";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day20(string[] input) => this.Input = input;

        public string Silver() => $"{new TrenchMap(this.Input).EnhanceImage(2)}";

        public string Gold() => $"{new TrenchMap(this.Input).EnhanceImage(50)}";
    }
}
