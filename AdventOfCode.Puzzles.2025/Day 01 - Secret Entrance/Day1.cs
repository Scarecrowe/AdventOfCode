namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_01___Secret_Entrance;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
        {
            this.DayTitle = "Secret Entrance";
            this.GetPuzzleData(1, this.DayTitle);
        }

        public string Silver() => $"{new SecretEntrance(this.Input).Password()}";

        public string Gold() => $"{new SecretEntrance(this.Input).AdvancedPassword()}";
    }
}
