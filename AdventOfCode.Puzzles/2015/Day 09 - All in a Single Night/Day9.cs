namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_09___All_in_a_Single_Night;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9(string file)
        {
            this.DayTitle = "All in a Single Night";
            this.GetPuzzleData(file);
        }

        public Day9(string[] input) => this.Input = input;

        public string Silver() => $"{new AllInASingleNight(this.Input).Shortest()}";

        public string Gold() => $"{new AllInASingleNight(this.Input).Longest()}";
    }
}
