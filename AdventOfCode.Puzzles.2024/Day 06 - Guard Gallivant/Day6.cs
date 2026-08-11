namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_06___Guard_Gallivant;

    public class Day6 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day6()
            : base(2024, 5, "Guard Gallivant")
        {
        }

        public Day6(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new GuardGallivant(this.Input).WithoutCollisions()}";

        public string Gold()
            => $"{new GuardGallivant(this.Input).WithCollisions()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new GuardGallivant(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new GuardGallivant(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => GuardGallivantTheme.ToAsciiConfiguration(this);
    }
}
