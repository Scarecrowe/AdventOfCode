namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_12___Digital_Plumber;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12(string file)
        {
            this.DayTitle = "Digital Plumber";
            this.GetPuzzleData(file);
        }

        public Day12(string[] input) => this.Input = input;

        public string Silver() => $"{new DigitalPlumber(this.Input).GroupCountByProgramId(0)}";

        public string Gold() => $"{new DigitalPlumber(this.Input).GroupCount()}";
    }
}
