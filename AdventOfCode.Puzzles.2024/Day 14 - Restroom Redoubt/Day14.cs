namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_14___Restroom_Redoubt;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14()
        {
            this.DayTitle = "Restroom Redoubt";
            this.GetPuzzleData(14, this.DayTitle);
        }

        public string Silver() => $"{new RestroomRedoubt(this.Input).SafetyFactor(100)}";

        public string Gold() => $"{new RestroomRedoubt(this.Input).XmasTree()}";
    }
}
