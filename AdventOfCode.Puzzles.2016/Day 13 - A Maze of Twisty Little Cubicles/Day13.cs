namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_13___A_Maze_of_Twisty_Little_Cubicles;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13()
        {
            this.DayTitle = "A Maze of Twisty Little Cubicles";
            this.GetPuzzleData(13, this.DayTitle);
        }

        public Day13(string[] input) => this.Input = input;

        public string Silver() => $"{new AMazeOfTwistyLittleCubicles(this.Input, 31, 39).FindAllPaths().ShortestPath()}";

        public string Gold() => $"{new AMazeOfTwistyLittleCubicles(this.Input, 31, 39).FindAllPaths().UniqueLocations()}";
    }
}
