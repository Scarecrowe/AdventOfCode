namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_22___Monkey_Map;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;

    public class Day22 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day22()
            : base(2022, 22, "Monkey Map", StringSplitOptions.None)
        {
        }

        public Day22(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new MonkeyMap(this.Input).Navigate(MonkeyMapType.TwoDimensional).Password()}";

        public string Gold() => $"{new MonkeyMap(this.Input).Navigate(MonkeyMapType.ThreeDimensional).Password()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new MonkeyMap(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new MonkeyMap(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => MonkeyMapTheme.ToAsciiConfiguration(this);
    }
}
