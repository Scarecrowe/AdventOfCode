namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_13___Transparent_Origami;

    public class Day13 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day13()
            : base(2021, 13, "Transparent Origami", StringSplitOptions.None)
        {
        }

        public Day13(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TransparentOrigami(this.Input).FoldOnce().Folds.ElementAt(0).Dots}";

        public string Gold() => $"{new TransparentOrigami(this.Input).Fold().Print()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new TransparentOrigami(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new TransparentOrigami(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TransparentOrigamiTheme.ToAsciiConfiguration(this);
    }
}
