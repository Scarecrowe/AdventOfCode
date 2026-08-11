namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_04___Scratchcards;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
            : base(2023, 4, "Scratchcards")
        {
        }

        public Day4(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Scratchcards(this.Input).TotalPoints()}";

        public string Gold() => $"{new Scratchcards(this.Input).TotalCards()}";
    }
}
