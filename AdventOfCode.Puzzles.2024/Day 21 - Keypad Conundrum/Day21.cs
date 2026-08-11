namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_21___Keypad_Conundrum;

    public class Day21 : Puzzle, IPuzzle
    {

        public Day21()
            : base(2024, 21, "Keypad Conundrum", StringSplitOptions.None)
        {
        }

        public Day21(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new KeypadConundrum(this.Input).Complexity(2)}";

        public string Gold() => $"{new KeypadConundrum(this.Input).Complexity(25)}";
    }
}
