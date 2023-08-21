namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_15___Beverage_Bandits;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15(string file)
        {
            this.DayTitle = "Beverage Bandits";
            this.GetPuzzleData(file);
        }

        public Day15(string[] input) => this.Input = input;

        [Slow]
        public string Silver() => $"{new BeverageBandits(this.Input).Battle()}";

        [Slow]
        public string Gold() => $"{new BeverageBandits(this.Input).BattleWithTechnology()}";
    }
}
