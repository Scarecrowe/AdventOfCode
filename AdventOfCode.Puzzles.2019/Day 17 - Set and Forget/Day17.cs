namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_17___Set_and_Forget;
    using AdventOfCode.Animation;

    public class Day17 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day17()
            : base(2019, 17, "Set and Forget")
        {
        }

        public Day17(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SetAndForget(this.Input[0]).BuildMap().AlignmentParameter}";

        public string Gold() => $"{new SetAndForget(this.Input[0]).BuildMap(2).FindRobots().DustCollected}";

        public void SilverFrame(IFrameRenderer renderer)
            => new SetAndForget(this.Input[0], renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new SetAndForget(this.Input[0], renderer).RenderGold(renderEvery: 2);

        public IAsciiRendererConfiguration AsciiConfiguration()
            => SetAndForgetTheme.ToAsciiConfiguration(this);
    }
}
