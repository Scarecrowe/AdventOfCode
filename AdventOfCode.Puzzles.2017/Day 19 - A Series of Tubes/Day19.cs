namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_19___A_Series_of_Tubes;

    public class Day19 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day19()
            : base(2017, 16, "A Series of Tubes")
        {
        }

        public Day19(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => new ASeriesOfTubes(this.Input).Move();

        public string Gold()
            => new ASeriesOfTubes(this.Input).Move(returnSteps: true);

        public void SilverFrame(IFrameRenderer renderer)
            => new ASeriesOfTubes(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ASeriesOfTubes(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ASeriesOfTubesTheme.ToAsciiConfiguration(this);
    }
}
