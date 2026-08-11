namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21()
            : base(2015, 21, "RPG Simulator 20XX")
        {
        }

        public Day21(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new RpgSimulator20XX(this.Input).MinGold()}";

        public string Gold() => $"{new RpgSimulator20XX(this.Input).MaxGold()}";
    }
}
