namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_10___Syntax_Scoring;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10(string file)
        {
            this.DayTitle = "Syntax Scoring";
            this.GetPuzzleData(file);
        }

        public Day10(string[] input) => this.Input = input;

        public string Silver() => $"{new SyntaxScoring(this.Input).ErrorScore()}";

        public string Gold() => $"{new SyntaxScoring(this.Input).MiddleScore()}";
    }
}
