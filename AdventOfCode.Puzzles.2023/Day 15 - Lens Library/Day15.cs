namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_15___Lens_Library;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15()
        {
            this.DayTitle = "Lens Library";
            this.GetPuzzleData(15, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new LensLibrary(this.Input).InstructionHashSum}";

        public string Gold() => $"{new LensLibrary(this.Input).FocusingPower()}";
    }
}
