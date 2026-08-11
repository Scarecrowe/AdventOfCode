namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_23___Unstable_Diffusion;

    public class Day23 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day23()
            : base(2022, 23, "Unstable Diffusion")
        {
        }

        public Day23(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new UnstableDiffusion(this.Input).Run().EmptyGround()}";

        [Slow]
        public string Gold() => $"{new UnstableDiffusion(this.Input).Run(true).Round}";

        public void SilverFrame(IFrameRenderer renderer)
            => new UnstableDiffusion(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new UnstableDiffusion(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
             => UnstableDiffusionTheme.ToAsciiConfiguration(this);
    }
}
