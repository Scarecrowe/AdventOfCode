namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_10___Pipe_Maze;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10()
        {
            this.DayTitle = "Pipe Maze";
            this.GetPuzzleData(10, this.DayTitle);
        }

        public string Silver() => $"{new PipeMaze(this.Input).Move()}";

        public string Gold() => $"{new PipeMaze(this.Input).Move()}";
    }
}
