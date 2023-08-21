namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_02___Corruption_Checksum;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2(string file)
        {
            this.DayTitle = "Corruption Checksum";
            this.GetPuzzleData(file);
        }

        public Day2(string[] input) => this.Input = input;

        public string Silver() => $"{new CorruptionChecksum(this.Input).SumOfLargestSmallest()}";

        public string Gold() => $"{new CorruptionChecksum(this.Input).SumOfEven()}";
    }
}
