namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_12___Rain_Risk;

    public class Day12 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day12()
            : base(2020, 12, "Rain Risk")
        {
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new RainRisk(this.Input).Distance()}";

        public string Gold() => $"{new RainRisk(this.Input).DistanceWithWaypoint()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new RainRisk(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new RainRisk(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => RainRiskTheme.ToAsciiConfiguration(this);
    }
}
