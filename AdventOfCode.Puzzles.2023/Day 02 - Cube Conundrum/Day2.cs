namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_02___Cube_Conundrum;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
            : base(2023, 2, "Cube Conundrum")
        {
        }

        public Day2(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CubeConundrum(this.Input).SumOfIds(12, 13, 14)}";

        public string Gold() => $"{new CubeConundrum(this.Input).SumOfPower()}";
    }
}
