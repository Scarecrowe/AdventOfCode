namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_09___Explosives_in_Cyberspace;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
            : base(2016, 9, "Explosives in Cyberspace")
        {
        }

        public Day9(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{ExplosivesInCyberspace.Decompress(this.Input[0])}";

        public string Gold() => $"{ExplosivesInCyberspace.Decompress(this.Input[0], 2)}";
    }
}
