namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_17___Two_Steps_Forward;

    public class Day17 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day17()
            : base(2016, 17, "Two Steps Forward")
        {
        }

        public Day17(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => new TwoStepsForward(this.Input[0]).ShortestPath();

        public string Gold()
            => $"{new TwoStepsForward(this.Input[0]).LongestPathLength()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new TwoStepsForward(this.Input[0], renderer)
                .RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new TwoStepsForward(this.Input[0], renderer)
                .RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TwoStepsForwardTheme.ToAsciiConfiguration(this);
    }
}