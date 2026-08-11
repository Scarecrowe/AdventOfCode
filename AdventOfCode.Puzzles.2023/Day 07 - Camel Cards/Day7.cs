namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_07___Camel_Cards;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
            : base(2023, 7, "Camel Cards")
        {
        }

        public Day7(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CamelCards(this.Input).Play()}";

        public string Gold() => $"{new CamelCards(this.Input, true).Play()}";
    }
}
