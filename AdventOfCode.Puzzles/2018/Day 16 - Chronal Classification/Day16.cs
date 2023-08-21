namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_16___Chronal_Classification;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16(string file)
        {
            this.DayTitle = "Chronal Classification";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day16(string[] input) => this.Input = input;

        public string Silver() => $"{new ChronalClassification(this.Input).TestSamples().Count(c => c.Count >= 3)}";

        public string Gold() => $"{new ChronalClassification(this.Input).Run()}";
    }
}
