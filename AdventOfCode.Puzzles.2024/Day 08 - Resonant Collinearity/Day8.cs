namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_08___Resonant_Collinearity;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8()
        {
            this.DayTitle = "Resonant Collinearity";
            this.GetPuzzleData(8, this.DayTitle);
        }

        public string Silver() => $"{new ResonantCollinearity(this.Input).AntiNodes().Count}";

        public string Gold() => $"{new ResonantCollinearity(this.Input).AntiNodes(true).Count}";
    }
}
