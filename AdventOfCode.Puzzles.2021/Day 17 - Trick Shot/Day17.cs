namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_17___Trick_Shot;

    public class Day17 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day17()
            : base(2021, 17, "Trick Shot")
        {
        }

        public Day17(string[] input)
            : this()
        {
            this.Input = input;
        }


        public string Silver() => $"{new TrickShot(this.Input).Simulate(true)}";

        public string Gold() => $"{new TrickShot(this.Input).Simulate(false)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new TrickShot(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => throw new NotImplementedException();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TrickShotTheme.ToAsciiConfiguration(this);
    }
}
