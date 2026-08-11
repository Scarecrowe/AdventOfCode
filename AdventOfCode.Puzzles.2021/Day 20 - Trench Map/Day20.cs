namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_20___Trench_Map;

    public class Day20 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day20()
            : base(2021, 20, "Trench Map", StringSplitOptions.None)
        {
        }

        public Day20(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TrenchMap(this.Input).EnhanceImage(2)}";

        public string Gold() => $"{new TrenchMap(this.Input).EnhanceImage(50)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new TrenchMap(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new TrenchMap(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TrenchMapTheme.ToAsciiConfiguration(this);
    }
}
