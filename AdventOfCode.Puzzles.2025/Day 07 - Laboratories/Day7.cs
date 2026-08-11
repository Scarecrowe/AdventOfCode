namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_07___Laboratories;

    public class Day7 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day7()
            : base(2025, 7, "Laboratories")
        {
        }

        public Day7(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new Laboratories(this.Input).SplitBeams()}";

        public string Gold()
            => $"{new Laboratories(this.Input).Timelines()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new Laboratories(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new Laboratories(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => LaboratoriesTheme.ToAsciiConfiguration(this);
    }
}
