namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_09___Rope_Bridge;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;

    public class Day9 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day9()
            : base(2022, 9, "Rope Bridge")
        {
        }

        public Day9(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new RopeBridge(this.Input, 2).Visited()}";

        public string Gold() => $"{new RopeBridge(this.Input, 10).Visited()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new RopeBridge(this.Input, 2, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new RopeBridge(this.Input, 10, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => RopeBridgeTheme.ToAsciiConfiguration(this);
    }
}
