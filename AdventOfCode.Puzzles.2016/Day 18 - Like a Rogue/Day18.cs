namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_18___Like_a_Rogue;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18()
            : base(2016, 18, "Like a Rogue")
        {
        }

        public Day18(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new LikeARogue(this.Input[0]).BuildMap(40).SafeTileCount}";

        [Slow]
        public string Gold() => $"{new LikeARogue(this.Input[0]).BuildMap(400000).SafeTileCount}";
    }
}
