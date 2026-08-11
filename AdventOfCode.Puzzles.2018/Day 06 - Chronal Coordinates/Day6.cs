namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_06___Chronal_Coordinates;

    public class Day6 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day6()
            : base(2018, 6, "Chronal Coordinates")
        {
        }

        public Day6(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ChronalCoordinates(this.Input).Dangerous()}";

        public string Gold()
            => $"{new ChronalCoordinates(this.Input).Safe(10000)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ChronalCoordinates(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ChronalCoordinates(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ChronalCoordinatesTheme.ToAsciiConfiguration(this);
    }
}