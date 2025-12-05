namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_04___Printing_Department;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
        {
            this.DayTitle = "Printing Department";
            this.GetPuzzleData(4, this.DayTitle);
        }

        public string Silver() => $"{new PrintingDepartment(this.Input).AccessableRolls()}";

        public string Gold() => $"{new PrintingDepartment(this.Input).AllAccessableRolls()}";
    }
}
