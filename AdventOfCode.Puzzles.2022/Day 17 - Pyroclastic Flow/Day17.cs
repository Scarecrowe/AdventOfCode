namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_17___Pyroclastic_Flow;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;

    public class Day17 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day17()
            : base(2022, 17, "Pyroclastic Flow")
        {
        }

        public Day17(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new PyroclasticFlow(this.Input).Stack(2022)}";

        public string Gold() => $"{new PyroclasticFlow(this.Input).Stack(1000000000000)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new PyroclasticFlow(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new PyroclasticFlow(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => PyroclasticFlowTheme.ToAsciiConfiguration(this);
    }
}
