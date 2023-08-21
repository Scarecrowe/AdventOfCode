namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_14___Space_Stoichiometry;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14(string file)
        {
            this.DayTitle = "Space Stoichiometry";
            this.GetPuzzleData(file);
        }

        public Day14(string[] input) => this.Input = input;

        public string Silver() => $"{new SpaceStoichiometry(this.Input).GetRequiredOre()}";

        public string Gold() => $"{new SpaceStoichiometry(this.Input).GetTotalOre()}";
    }
}
