namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_04___Repose_Record;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4(string file)
        {
            this.DayTitle = "Repose Record";
            this.GetPuzzleData(file);
        }

        public Day4(string[] input) => this.Input = input;

        public string Silver() => $"{new ReposeRecord(this.Input).ProcessLog().StrategyOne()}";

        public string Gold() => $"{new ReposeRecord(this.Input).ProcessLog().StrategyTwo()}";
    }
}
