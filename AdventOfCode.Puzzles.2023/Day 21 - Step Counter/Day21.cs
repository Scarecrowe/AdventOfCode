namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_21___Step_Counter;

    public class Day21 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day21()
            : base(2023, 21, "Step Counter")
        {
        }

        public Day21(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new StepCounter(this.Input).ShortWalk()}";

        public string Gold()
            => $"{new StepCounter(this.Input).LongWalk()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new StepCounter(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new StepCounter(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => StepCounterTheme.ToAsciiConfiguration(this);
    }
}
