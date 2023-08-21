namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_17___Set_and_Forget;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17(string file)
        {
            this.DayTitle = "Set and Forget";
            this.GetPuzzleData(file);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{new SetAndForget(this.Input[0]).BuildMap().AlignmentParameter}";

        public string Gold() => $"{new SetAndForget(this.Input[0]).BuildMap(2).FindRobots().DustCollected}";
    }
}
