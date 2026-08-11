namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_02___1202_Program_Alarm;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
            : base(2019, 2, "1202 Program Alarm")
        {
        }

        public Day2(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new OneTwoOneTwoProgramAlarm(this.Input[0]).Run(12, 2)}";

        public string Gold() => $"{new OneTwoOneTwoProgramAlarm(this.Input[0]).Run()}";
    }
}
