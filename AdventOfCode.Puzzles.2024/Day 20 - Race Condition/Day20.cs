namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_20___Race_Condition;

    public class Day20 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day20()
            : base(2024, 20, "Race Condition")
        {
        }

        public Day20(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new RaceCondition(this.Input).NormalRace()}";

        public string Gold()
            => $"{new RaceCondition(this.Input).ExtendedRace()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new RaceCondition(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new RaceCondition(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => RaceConditionTheme.ToAsciiConfiguration(this);
    }
}
