namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_06___Probably_a_Fire_Hazard;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6(string file)
        {
            this.DayTitle = "Probably a Fire Hazard";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day6(string[] input) => this.Input = input;

        public string Silver() => $"{new ProbablyAFireHazard(this.Input, LightBrightness.Single).Lit()}";

        public string Gold() => $"{new ProbablyAFireHazard(this.Input, LightBrightness.Multiple).Lit()}";
    }
}
