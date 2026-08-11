namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_23___Amphipod;

    public class Day23 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day23()
            : base(2021, 23, "Amphipod")
        {
        }

        public Day23(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Amphipod(this.Input).Run()}";

        public string Gold() => $"{new Amphipod(this.Input, true).Run()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new Amphipod(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new Amphipod(this.Input, renderer, true).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => AmphipodTheme.ToAsciiConfiguration(this);
    }
}
