namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_24___Blizzard_Basin;
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;

    public class Day24 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day24()
            : base(2022, 24, "Blizzard Basin")
        {
        }

        public Day24(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new BlizzardBasin(this.Input).FewestMinutes()}";

        public string Gold() => $"{new BlizzardBasin(this.Input).FewestMinutesWithRoundTrip()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new BlizzardBasin(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new BlizzardBasin(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => BlizzardBasinTheme.ToAsciiConfiguration(this);
    }
}
