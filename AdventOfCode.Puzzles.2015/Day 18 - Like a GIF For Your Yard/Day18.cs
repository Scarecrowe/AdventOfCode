namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_18___Like_a_GIF_For_Your_Yard;

    public class Day18 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day18()
            : base(2015, 18, "Like a GIF For Your Yard")
        {
        }

        public Day18(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new LikeAGIFForYourYard(this.Input).Animate(100).CountLit()}";

        public string Gold()
            => $"{new LikeAGIFForYourYard(this.Input).AnimateGameOfLife(100).CountLit()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new LikeAGIFForYourYard(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new LikeAGIFForYourYard(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => LikeAGIFForYourYardTheme.ToAsciiConfiguration(this);
    }
}
