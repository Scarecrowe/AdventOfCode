namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_20___A_Regular_Map;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20()
            : base(2018, 20, "A Regular Map")
        {
        }

        public Day20(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ARegularMap(this.Input).BuildMap().Furthest()}";

        public string Gold() => $"{new ARegularMap(this.Input).BuildMap().Shortest()}";
    }
}
