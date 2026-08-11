namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_13___Mine_Cart_Madness;

    public class Day13 : Puzzle, IPuzzle// , IAsciiAnimation
    {
        public Day13()
            : base(2018, 13, "Mine Cart Madness")
        {
        }

        public Day13(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new MineCartMadness(this.Input).Tick()}";

        public string Gold()
            => $"{new MineCartMadness(this.Input).Tick(false)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new MineCartMadness(this.Input, renderer).RenderSilver(renderEvery: 1);

        public void GoldFrame(IFrameRenderer renderer)
            => new MineCartMadness(this.Input, renderer).RenderGold(renderEvery: 1);

        public IAsciiRendererConfiguration AsciiConfiguration()
            => MineCartMadnessTheme.ToAsciiConfiguration(this);
    }
}
