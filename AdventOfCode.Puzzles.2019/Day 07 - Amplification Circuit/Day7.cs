namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_07___Amplification_Circuit;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
        {
            this.DayTitle = "Amplification Circuit";
            this.GetPuzzleData(7, this.DayTitle);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new AmplificationCircuit(this.Input).HighestThrusterSignal()}";

        public string Gold() => $"{new AmplificationCircuit(this.Input).HighestFeedbackThrusterSignal()}";
    }
}
