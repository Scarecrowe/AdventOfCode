namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_17___Conway_Cubes;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17()
            : base(2020, 17, "Conway Cubes", StringSplitOptions.None)
        {
        }

        public Day17(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{ConwayCubes.Simple(this.Input)}";

        public string Gold() => $"{ConwayCubes.Advanced(this.Input)}";
    }
}
