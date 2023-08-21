namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_23___Unstable_Diffusion;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23(string file)
        {
            this.DayTitle = "Unstable Diffusion";
            this.GetPuzzleData(file);
        }

        public Day23(string[] input) => this.Input = input;

        public string Silver() => $"{new UnstableDiffusion(this.Input).Run().EmptyGround()}";

        [Slow]
        public string Gold() => $"{new UnstableDiffusion(this.Input).Run(true).Round}";
    }
}
