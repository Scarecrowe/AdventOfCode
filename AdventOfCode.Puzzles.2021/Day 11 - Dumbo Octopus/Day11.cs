namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_11___Dumbo_Octopus;

    public class Day11 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day11()
            : base(2021, 11, "Dumbo Octopus")
        {
        }

        public Day11(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Cavern(this.Input).RunFor(100).Map.Flashes}";

        public string Gold() => $"{new Cavern(this.Input).RunUntil()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new Cavern(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new Cavern(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => DumboOctopusTheme.ToAsciiConfiguration(this);
    }
}
