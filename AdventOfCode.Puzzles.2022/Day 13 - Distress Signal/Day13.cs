namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_13___Distress_Signal;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13()
            : base(2022, 13, "Distress Signal")
        {
        }

        public Day13(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new DistressSignal(this.Input).SumOfIndices()}";

        public string Gold() => $"{new DistressSignal(this.Input).DecoderKey()}";
    }
}
