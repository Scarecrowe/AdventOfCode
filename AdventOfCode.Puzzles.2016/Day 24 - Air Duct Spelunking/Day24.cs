namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_24___Air_Duct_Spelunking;

    public class Day24 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day24()
            : base(2016, 24, "Air Duct Spelunking")
        {
        }

        public Day24(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new AirDuctSpelunking(this.Input).ShortestPath()}";

        public string Gold()
            => $"{new AirDuctSpelunking(this.Input).ShortestPath(true)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new AirDuctSpelunking(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new AirDuctSpelunking(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => AirDuctSpelunkingTheme.ToAsciiConfiguration(this);
    }
}
