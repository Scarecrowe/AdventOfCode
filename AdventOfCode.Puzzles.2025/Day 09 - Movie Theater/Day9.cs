namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_09___Movie_Theater;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
        {
            this.DayTitle = "Movie Theater";
            this.GetPuzzleData(9, this.DayTitle);
        }

        public string Silver() => $"{new MovieTheater(this.Input).AreaOutside()}";

        public string Gold() => $"{new MovieTheater(this.Input).AreaInside()}";

    }
}
