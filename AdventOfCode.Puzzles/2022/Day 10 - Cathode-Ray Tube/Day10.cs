namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_10___Cathode_Ray_Tube;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10(string file)
        {
            this.DayTitle = "Cathode-Ray Tube";
            this.GetPuzzleData(file);
        }

        public Day10(string[] input) => this.Input = input;

        public string Silver() => $"{new CathodeRayTube(this.Input).Strength}";

        public string Gold() => $"{new CathodeRayTube(this.Input).Display}";
    }
}
