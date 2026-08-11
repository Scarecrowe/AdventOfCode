namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_12___Subterranean_Sustainability;

    public class Day12 : Puzzle, IPuzzle// , IAsciiAnimation
    {
        public Day12()
            : base(2018, 12, "Subterranean Sustainability")
        {
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new SubterraneanSustainability(this.Input).Grow(20)}";

        public string Gold()
            => $"{new SubterraneanSustainability(this.Input).Grow(50000000000)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new SubterraneanSustainability(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new SubterraneanSustainability(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => SubterraneanSustainabilityTheme.ToAsciiConfiguration(this);
    }
}
