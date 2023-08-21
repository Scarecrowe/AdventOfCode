namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_20___Donut_Maze;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20(string file)
        {
            this.DayTitle = "Donut Maze";
            this.GetPuzzleData(file);
        }

        public Day20(string[] input) => this.Input = input;

        public string Silver() => $"{new DonutMaze(this.Input).Search()}";

        public string Gold() => $"{new DonutMaze(this.Input).Search(true)}";
    }
}
