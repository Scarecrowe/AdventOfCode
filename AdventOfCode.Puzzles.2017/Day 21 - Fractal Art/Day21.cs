namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_21___Fractal_Art;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21()
            : base(2017, 21, "Fractal Art")
        {
        }

        public Day21(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new FractalArt(this.Input).Enhance(5).Pattern.PixelCount()}";

        public string Gold() => $"{new FractalArt(this.Input).Enhance(18).Pattern.PixelCount()}";
    }
}
