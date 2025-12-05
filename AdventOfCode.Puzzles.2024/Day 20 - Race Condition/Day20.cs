namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_20___Race_Condition;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20()
        {
            this.DayTitle = "Race Condition";
            this.GetPuzzleData(20, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new RaceCondition(this.Input).NormalRace()}";

        public string Gold() => $"{new RaceCondition(this.Input).ExtendedRace()}";
    }
}
