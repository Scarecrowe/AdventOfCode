namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_10___Cathode_Ray_Tube;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10()
            : base(2022, 10, "Cathode-Ray Tube")
        {
        }

        public Day10(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CathodeRayTube(this.Input).Strength}";

        public string Gold() => $"{new CathodeRayTube(this.Input).Display}";
    }
}
