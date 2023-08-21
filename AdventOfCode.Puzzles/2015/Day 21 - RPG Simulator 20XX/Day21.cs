namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21(string file)
        {
            this.DayTitle = "RPG Simulator 20XX";
            this.GetPuzzleData(file);
        }

        public Day21(string[] input) => this.Input = input;

        public string Silver() => $"{new RpgSimulator20XX(this.Input).MinGold()}";

        public string Gold() => $"{new RpgSimulator20XX(this.Input).MaxGold()}";
    }
}
