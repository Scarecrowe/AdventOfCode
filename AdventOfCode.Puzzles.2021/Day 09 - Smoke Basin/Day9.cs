namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_09___Smoke_Basin;
    using AdventOfCode.Animation;

    public class Day9 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day9()
            : base(2021, 9, "Smoke Basin")
        {
        }

        public Day9(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SmokeBasin(this.Input).SumOfRiskLevels()}";

        public string Gold() => $"{new SmokeBasin(this.Input).SumOfBasin()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new SmokeBasin(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new SmokeBasin(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => SmokeBasinTheme.ToAsciiConfiguration(this);
    }
}
