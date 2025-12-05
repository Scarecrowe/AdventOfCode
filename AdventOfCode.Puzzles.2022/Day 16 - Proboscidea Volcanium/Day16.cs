namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_16___Proboscidea_Volcanium;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16()
        {
            this.DayTitle = "Proboscidea Volcanium";
            this.GetPuzzleData(16, this.DayTitle);
        }

        public Day16(string[] input) => this.Input = input;

        public string Silver() => $"{new ProboscideaVolcanium(this.Input).Single()}";

        public string Gold() => $"{new ProboscideaVolcanium(this.Input).Pair()}";
    }
}
