namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_04___Ceres_Search;

    public class Day4 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day4()
            : base(2024, 4, "Ceres Search")
        {
        }

        public Day4(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new CeresSearch(this.Input).XmasCount()}";

        public string Gold()
            => $"{new CeresSearch(this.Input).XmasHyphenCount()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new CeresSearch(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new CeresSearch(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => CeresSearchTheme.ToAsciiConfiguration(this);
    }
}
