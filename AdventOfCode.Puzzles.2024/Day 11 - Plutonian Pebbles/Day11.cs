namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_11___Plutonian_Pebbles;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
        {
            this.DayTitle = "Plutonian Pebbles";
            this.GetPuzzleData(11, this.DayTitle);
        }

        public string Silver() => $"{new PlutonianPebbles().Blink(this.Input, 25)}";

        public string Gold() => $"{new PlutonianPebbles().Blink(this.Input, 75)}";
    }
}
