namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_19___Tractor_Beam;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19(string file)
        {
            this.DayTitle = "Tractor Beam";
            this.GetPuzzleData(file);
        }

        public Day19(string[] input) => this.Input = input;

        public string Silver() => $"{new TractorBeam(this.Input[0]).BuildMap(50, 0, 50).TractorBeamArea}";

        [Slow]
        public string Gold() => $"{new TractorBeam(this.Input[0]).BuildMap(1000, 700, 1200).ClosestPoint()}";
    }
}
