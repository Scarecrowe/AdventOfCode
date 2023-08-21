namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_15___Rambunctious_Recitation;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15(string file)
        {
            this.DayTitle = "Rambunctious Recitation";
            this.GetPuzzleData(file);
        }

        public Day15(string[] input) => this.Input = input;

        public string Silver() => $"{new RambunctiousRecitation(this.Input).LastSpoken(2020)}";

        public string Gold() => $"{new RambunctiousRecitation(this.Input).LastSpoken(30000000)}";
    }
}
