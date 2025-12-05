namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_21___Step_Counter;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21()
        {
            this.DayTitle = "Step Counter";
            this.GetPuzzleData(21, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new StepCounter(this.Input).ShortWalk()}";

        public string Gold() => $"{new StepCounter(this.Input).LongWalk()}";
    }
}
