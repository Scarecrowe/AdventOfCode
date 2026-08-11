namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_03___Crossed_Wires;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
            : base(2019, 3, "Crossed Wires")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CrossedWires(this.Input).Nearest()}";

        public string Gold() => $"{new CrossedWires(this.Input).StepsToIntersection()}";
    }
}
