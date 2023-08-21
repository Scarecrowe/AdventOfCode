namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_08___Matchsticks;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8(string file)
        {
            this.DayTitle = "Matchsticks";
            this.GetPuzzleData(file, StringSplitOptions.RemoveEmptyEntries, false);
        }

        public Day8(string[] input) => this.Input = input;

        public string Silver() => $"{Matchsticks.CountCharacters(this.Input[0], MatchstickMode.Normal)}";

        public string Gold() => $"{Matchsticks.CountCharacters(this.Input[0], MatchstickMode.Escaped)}";
    }
}
