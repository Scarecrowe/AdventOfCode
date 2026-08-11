namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_24___Immune_System_Simulator_20XX;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24()
            : base(2018, 24, "Immune System Simulator 20XX", StringSplitOptions.None)
        {
        }

        public Day24(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ImmuneSystemSimulator20XX(this.Input).Battle()}";

        public string Gold() => $"{new ImmuneSystemSimulator20XX(this.Input).BattleWithBoost()}";
    }
}
