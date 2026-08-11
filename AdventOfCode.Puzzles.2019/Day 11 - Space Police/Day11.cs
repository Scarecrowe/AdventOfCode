namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_11___Space_Police;

    public class Day11 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day11()
            : base(2019, 11, "Space Police", StringSplitOptions.None)
        {
        }

        public Day11(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new SpacePolice(this.Input[0]).PaintHull(0, 0, PaintColour.Black).Hull.Count}";

        public string Gold()
            => $"{new SpacePolice(this.Input[0]).PaintHull(0, 1, PaintColour.White).Print()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new SpacePolice(this.Input[0], renderer).RenderPaintHull(0, 0, PaintColour.Black, renderEvery: 25);

        public void GoldFrame(IFrameRenderer renderer)
            => new SpacePolice(this.Input[0], renderer).RenderPaintHull(0, 1, PaintColour.White, renderEvery: 1);

        public IAsciiRendererConfiguration AsciiConfiguration()
            => SpacePoliceTheme.ToAsciiConfiguration(this);
    }
}
