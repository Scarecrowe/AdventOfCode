namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_24___Arithmetic_Logic_Unit;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24(string file)
        {
            this.DayTitle = "Arithmetic Logic Unit";
            this.GetPuzzleData(file);
        }

        public Day24(string[] input) => this.Input = input;

        public string Silver() => $"{new ArithmeticLogicUnit(this.Input).Largest()}";

        public string Gold() => $"{new ArithmeticLogicUnit(this.Input).Smallest()}";
    }
}
