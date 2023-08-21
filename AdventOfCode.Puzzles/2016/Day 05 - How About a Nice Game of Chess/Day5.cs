namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_05___How_About_a_Nice_Game_of_Chess;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5(string file)
        {
            this.DayTitle = "How About a Nice Game of Chess";
            this.GetPuzzleData(file);
        }

        public Day5(string[] input) => this.Input = input;

        [Slow]
        public string Silver() => $"{HowAboutANiceGameOfChess.Simple(this.Input[0])}";

        [Slow]
        public string Gold() => $"{HowAboutANiceGameOfChess.Advanced(this.Input[0])}";
    }
}
