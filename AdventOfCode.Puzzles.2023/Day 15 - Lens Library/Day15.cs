namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_15___Lens_Library;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15()
            : base(2023, 15, "Lens Library", StringSplitOptions.None)
        {
        }

        public Day15(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new LensLibrary(this.Input).InstructionHashSum}";

        public string Gold() => $"{new LensLibrary(this.Input).FocusingPower()}";
    }
}
