namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_15___Chiton;

    public class Day15 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day15()
            : base(2021, 15, "Chiton", StringSplitOptions.None)
        {
        }

        public Day15(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ChitonNavigator(this.Input).Navigate()}";

        public string Gold() => $"{new ChitonNavigator(this.Input).Enlarge().Navigate()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ChitonNavigator(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ChitonNavigator(this.Input, renderer).Enlarge().RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ChitonTheme.ToAsciiConfiguration(this);
    }
}
