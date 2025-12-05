namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_24___Arithmetic_Logic_Unit;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24()
        {
            this.DayTitle = "Arithmetic Logic Unit";
            this.GetPuzzleData(24, this.DayTitle);
        }

        public Day24(string[] input) => this.Input = input;

        public string Silver() => $"{new ArithmeticLogicUnit(this.Input).Largest()}";

        public string Gold() => $"{new ArithmeticLogicUnit(this.Input).Smallest()}";
    }
}
