namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_18___Lavaduct_Lagoon;

    public class Day18 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day18()
            : base(2023, 18, "Lavaduct Lagoon")
        {
        }

        public Day18(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new LavaductLagoon(this.Input).Small()}";

        public string Gold()
            => $"{new LavaductLagoon(this.Input).Large()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new LavaductLagoon(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new LavaductLagoon(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => LavaductLagoonTheme.ToAsciiConfiguration(this);
    }
}
