namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_19___Monster_Messages;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19()
        {
            this.DayTitle = "Monster Messages";
            this.GetPuzzleData(19, this.DayTitle, StringSplitOptions.None);
        }

        public Day19(string[] input) => this.Input = input;

        public string Silver() => $"{new MonsterMessages(this.Input).Simple()}";

        public string Gold() => $"{new MonsterMessages(this.Input).Advanced()}";
    }
}
