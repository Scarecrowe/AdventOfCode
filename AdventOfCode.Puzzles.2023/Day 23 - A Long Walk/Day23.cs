namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_23___A_Long_Walk;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23()
        {
            this.DayTitle = "A Long Walk";
            this.GetPuzzleData(23, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new ALongWalk(this.Input).LongestHike()}";

        public string Gold() => $"{new ALongWalk(this.Input).UniqueLongestHike()}";
    }
}
