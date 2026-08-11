namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_25___Four_Dimensional_Adventure;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25()
            : base(2018, 25, "Four-Dimensional Adventure")
        {
        }

        public Day25(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new FourDimensionalAdventure(this.Input).ConstellationCount()}";

        public string Gold() => $"You have enough stars to [Trigger the Underflow]";
    }
}
