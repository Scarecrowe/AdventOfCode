namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_13___Point_of_Incidence;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13()
        {
            this.DayTitle = "Point of Incidence";
            this.GetPuzzleData(13, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new PointOfIncidence(this.Input)}";

        public string Gold() => $"{new PointOfIncidence(this.Input)}";
    }
}
