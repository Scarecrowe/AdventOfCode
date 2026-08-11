namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_11___Plutonian_Pebbles;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
            : base(2024, 11, "Plutonian Pebbles")
        {
        }

        public Day11(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new PlutonianPebbles().Blink(this.Input, 25)}";

        public string Gold() => $"{new PlutonianPebbles().Blink(this.Input, 75)}";
    }
}
