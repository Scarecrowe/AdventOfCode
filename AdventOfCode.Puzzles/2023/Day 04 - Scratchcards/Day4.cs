namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_04___Scratchcards;

    public class Day4 : Puzzle, IPuzzle
    {
        private string silver = string.Empty;

        private string gold = string.Empty;

        public Day4(string file)
        {
            this.DayTitle = "Scratchcards";
            this.GetPuzzleData(file);
        }

        public string Silver() => $"{new Scratchcards(this.Input).TotalPoints()}";

        public string Gold() => $"{new Scratchcards(this.Input).TotalCards()}";
    }
}
