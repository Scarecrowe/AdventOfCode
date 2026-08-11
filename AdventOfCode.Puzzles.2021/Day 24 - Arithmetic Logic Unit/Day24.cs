namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_24___Arithmetic_Logic_Unit;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24()
            : base(2021, 24, "Arithmetic Logic Unit")
        {
        }

        public Day24(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ArithmeticLogicUnit(this.Input).Largest()}";

        public string Gold() => $"{new ArithmeticLogicUnit(this.Input).Smallest()}";
    }
}
