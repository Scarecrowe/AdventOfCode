namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_04___Camp_Cleanup;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4(string file)
        {
            this.DayTitle = "Camp Cleanup";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day4(string[] input) => this.Input = input;

        public string Silver() => $"{CampCleanup.Contains(this.Input)}";

        public string Gold() => $"{CampCleanup.Overlap(this.Input)}";
    }
}
