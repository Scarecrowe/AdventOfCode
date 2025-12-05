namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_03___Toboggan_Trajectory;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
        {
            this.DayTitle = "Toboggan Trajectory";
            this.GetPuzzleData(3, this.DayTitle);
        }

        public Day3(string[] input) => this.Input = input;

        public string Silver() => $"{new TobogganTrajectory(this.Input).Single()}";

        public string Gold() => $"{new TobogganTrajectory(this.Input).Multiple()}";
    }
}
