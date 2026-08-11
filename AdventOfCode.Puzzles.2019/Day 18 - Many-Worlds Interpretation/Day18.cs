namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_18___Many_Worlds_Interpretation;

    public class Day18 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day18()
            : base(2019, 18, "Many-Worlds Interpretation")
        {
        }

        public Day18(string[] input)
            : this()
        {
            this.Input = input;
        }

        [Slow]
        public string Silver() => $"{new ManyWorldsInterpretation(this.Input).CollectKeys()}";

        public string Gold() => $"{new ManyWorldsInterpretation(this.Input).SplitMap().CollectVaultKeys()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ManyWorldsInterpretation(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ManyWorldsInterpretation(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ManyWorldsInterpretationTheme.ToAsciiConfiguration(this);
    }
}
