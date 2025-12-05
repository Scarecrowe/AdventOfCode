namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_22___Mode_Maze;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
        {
            this.DayTitle = "Mode Maze";
            this.GetPuzzleData(22, this.DayTitle);
        }

        public Day22(string[] input) => this.Input = input;

        public string Silver() => $"{new ModeMaze(this.Input, 0, 0).BuildMap().CountOfWetAndNarrow()}";

        [Slow]
        public string Gold() => $"{new ModeMaze(this.Input, 15, 15).BuildMap().WalkMaze()}";
    }
}
