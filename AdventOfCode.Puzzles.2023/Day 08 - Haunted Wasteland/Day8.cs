namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_08___Haunted_Wasteland;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8()
        {
            this.DayTitle = "Haunted Wasteland";
            this.GetPuzzleData(8, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new HauntedWasteland(this.Input).Move()}";

        public string Gold() => $"{new HauntedWasteland(this.Input).MoveGhost()}";
    }
}
