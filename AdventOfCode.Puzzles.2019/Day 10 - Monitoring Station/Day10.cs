namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_10___Monitoring_Station;

    public class Day10 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day10()
            : base(2019, 10, "Monitoring Station")
        {
        }

        public Day10(string[] input)
            : this()
        {
            this.Input = input;
        }

        [Slow]
        public string Silver() => $"{new MonitoringStation(this.Input).FindBestLocation().MaxTargets}";

        [Slow]
        public string Gold() => $"{new MonitoringStation(this.Input).FindBestLocation().ClearAsteroidField().VaporizedScore()}";

        public void SilverFrame(IFrameRenderer renderer) => new MonitoringStation(this.Input, renderer).Render();

        public void GoldFrame(IFrameRenderer renderer) => new MonitoringStation(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration() => MonitoringStationTheme.ToAsciiConfiguration(this);
    }
}
