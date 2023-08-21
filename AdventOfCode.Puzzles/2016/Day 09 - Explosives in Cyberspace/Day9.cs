namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_09___Explosives_in_Cyberspace;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9(string file)
        {
            this.DayTitle = "Explosives in Cyberspace";
            this.GetPuzzleData(file);
        }

        public Day9(string[] input) => this.Input = input;

        public string Silver() => $"{ExplosivesInCyberspace.Decompress(this.Input[0])}";

        public string Gold() => $"{ExplosivesInCyberspace.Decompress(this.Input[0], 2)}";
    }
}
