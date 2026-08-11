namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_25___Cryostasis;
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.Themes;

    public class Day25 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day25()
            : base(2019, 25, "Cryostasis", StringSplitOptions.None)
        {
        }

        public Day25(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Cryostasis(this.Input[0]).Run()}";

        public string Gold() => $"You have enough stars to [Align the Warp Drive]";

        public void SilverFrame(IFrameRenderer renderer)
            => new Cryostasis(this.Input[0], renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => throw new NotImplementedException();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => CryostasisTheme.ToAsciiConfiguration(this);
    }
}
