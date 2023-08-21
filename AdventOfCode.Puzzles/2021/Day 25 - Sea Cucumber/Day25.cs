namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_25___Sea_Cucumber;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25(string file)
        {
            this.DayTitle = "Sea Cucumber";
            this.GetPuzzleData(file);
        }

        public Day25(string[] input) => this.Input = input;

        public string Silver() => $"{new SeaCucumber(this.Input).Run()}";

        public string Gold() => $"You have enough stars to [Remotely Start The Sleigh]";
    }
}
