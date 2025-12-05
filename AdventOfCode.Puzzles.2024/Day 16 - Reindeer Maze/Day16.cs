namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_16___Reindeer_Maze;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16()
        {
            this.DayTitle = "Reindeer Maze";
            this.GetPuzzleData(16, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new ReindeerMaze(this.Input).BestScore()}";

        public string Gold() => $"{new ReindeerMaze(this.Input).Seats()}";
    }
}
