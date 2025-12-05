namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_07___Camel_Cards;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
        {
            this.DayTitle = "Camel Cards";
            this.GetPuzzleData(7, this.DayTitle);
        }

        public string Silver() => $"{new CamelCards(this.Input).Play()}";

        public string Gold() => $"{new CamelCards(this.Input, true).Play()}";
    }
}
