namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_07___The_Treachery_of_Whales;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
            : base(2021, 7, "The Treachery of Whales")
        {
        }

        public Day7(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TheTreacheryOfWhales(this.Input).Calculate(true)}";

        public string Gold() => $"{new TheTreacheryOfWhales(this.Input).Calculate(false)}";
    }
}
