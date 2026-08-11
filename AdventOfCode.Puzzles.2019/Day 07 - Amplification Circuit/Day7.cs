namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_07___Amplification_Circuit;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
            : base(2019, 7, "Amplification Circuit")
        {
        }

        public Day7(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new AmplificationCircuit(this.Input).HighestThrusterSignal()}";

        public string Gold() => $"{new AmplificationCircuit(this.Input).HighestFeedbackThrusterSignal()}";
    }
}
