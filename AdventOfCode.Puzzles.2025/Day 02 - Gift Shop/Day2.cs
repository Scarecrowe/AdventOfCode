namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_02___Gift_Shop;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
        {
            this.DayTitle = "Gift Shop";
            this.GetPuzzleData(2, this.DayTitle);
        }

        public string Silver() => $"{new GiftShop(this.Input).SumInvalidIds()}";

        public string Gold() => $"{new GiftShop(this.Input).SumInvalidIds(false)}";
    }
}
