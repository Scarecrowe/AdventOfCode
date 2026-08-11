namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_02___Gift_Shop;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
            : base(2025, 2, "Gift Shop")
        {
        }

        public Day2(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new GiftShop(this.Input).SumInvalidIds()}";

        public string Gold() => $"{new GiftShop(this.Input).SumInvalidIds(false)}";
    }
}
