namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_02___Bathroom_Security;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2(string file)
        {
            this.DayTitle = "Bathroom Security";
            this.GetPuzzleData(file);
        }

        public Day2(string[] input) => this.Input = input;

        public string Silver() => $"{new BathroomSecurity(this.Input, KeyPadMode.Simple).KeyCode()}";

        public string Gold() => $"{new BathroomSecurity(this.Input, KeyPadMode.Advanced).KeyCode()}";
    }
}
