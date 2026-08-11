namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_02___Bathroom_Security;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
            : base(2016, 2, "Bathroom Security")
        {
        }

        public Day2(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new BathroomSecurity(this.Input, KeyPadMode.Simple).KeyCode()}";

        public string Gold() => $"{new BathroomSecurity(this.Input, KeyPadMode.Advanced).KeyCode()}";
    }
}
