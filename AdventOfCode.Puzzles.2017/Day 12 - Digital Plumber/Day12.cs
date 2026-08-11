namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_12___Digital_Plumber;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12()
            : base(2017, 12, "Digital Plumber")
        {
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new DigitalPlumber(this.Input).GroupCountByProgramId(0)}";

        public string Gold() => $"{new DigitalPlumber(this.Input).GroupCount()}";
    }
}
