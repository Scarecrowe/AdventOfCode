namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_25___Sea_Cucumber;

    public class Day25 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day25()
            : base(2021, 25, "Sea Cucumber")
        {
        }

        public Day25(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SeaCucumber(this.Input).Run()}";

        public string Gold() => $"You have enough stars to [Remotely Start The Sleigh]";

        public void SilverFrame(IFrameRenderer renderer)
            => new SeaCucumber(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new SeaCucumber(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => SeaCucumberTheme.ToAsciiConfiguration(this);
    }
}
