namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_10___Cathode_Ray_Tube;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10()
        {
            this.DayTitle = "Cathode-Ray Tube";
            this.GetPuzzleData(10, this.DayTitle);
        }

        public Day10(string[] input) => this.Input = input;

        public string Silver() => $"{new CathodeRayTube(this.Input).Strength}";

        public string Gold() => $"{new CathodeRayTube(this.Input).Display}";
    }
}
