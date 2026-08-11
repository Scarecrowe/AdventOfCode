namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_12___Hot_Springs;

    public class Day12 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day12()
            : base(2023, 12, "Hot Springs")
        {
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new HotSprings(this.Input).Arrangements()}";

        public string Gold()
            => $"{new HotSprings(this.Input).UnfoldedArrangements()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new HotSprings(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new HotSprings(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => HotSpringsTheme.ToAsciiConfiguration(this);
    }
}
