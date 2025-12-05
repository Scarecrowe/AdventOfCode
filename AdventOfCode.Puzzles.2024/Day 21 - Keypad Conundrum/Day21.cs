namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_21___Keypad_Conundrum;

    public class Day21 : Puzzle, IPuzzle
    {

        public Day21()
        {
            this.DayTitle = "Keypad Conundrum";
            this.GetPuzzleData(21, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new KeypadConundrum(this.Input).Complexity(2)}";

        public string Gold() => $"{new KeypadConundrum(this.Input).Complexity(25)}";
    }
}
