namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_11___Chronal_Charge;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
        {
            this.DayTitle = "Chronal Charge";
            this.GetPuzzleData(11, this.DayTitle);
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{new ChronalCharger(this.Input[0]).MaxPowerLevel3x3()}";

        [Slow]
        public string Gold() => $"{new ChronalCharger(this.Input[0]).MaxPowerLevel()}";
    }
}
