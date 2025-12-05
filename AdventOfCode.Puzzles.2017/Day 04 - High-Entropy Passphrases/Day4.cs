namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_04___High_Entropy_Passphrases;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
        {
            this.DayTitle = "High-Entropy Passphrases";
            this.GetPuzzleData(4, this.DayTitle);
        }

        public Day4(string[] input) => this.Input = input;

        public string Silver() => $"{HighEntropyPassphrases.Simple(this.Input)}";

        public string Gold() => $"{HighEntropyPassphrases.Advanced(this.Input)}";
    }
}
