namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_20___Firewall_Rules;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20(string file)
        {
            this.DayTitle = "Firewall Rules";
            this.GetPuzzleData(file);
        }

        public Day20(string[] input) => this.Input = input;

        public string Silver() => $"{new FirewallRules(this.Input).LowesetValuedIP()}";

        public string Gold() => $"{new FirewallRules(this.Input).CountOfAllowedIP()}";
    }
}
