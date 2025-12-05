namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_22___Sporifica_Virus;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
        {
            this.DayTitle = "Sporifica Virus";
            this.GetPuzzleData(22, this.DayTitle);
        }

        public Day22(string[] input) => this.Input = input;

        public string Silver() => $"{new SporificaVirus(this.Input).Run(10000).InfectedCount}";

        [Slow]
        public string Gold() => $"{new SporificaVirus(this.Input).Run(10000000, true).InfectedCount}";
    }
}
