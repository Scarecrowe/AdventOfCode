namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_25___Code_Chronicle;

    public class Day25 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day25()
            : base(2024, 25, "Code Chronicle")
        {
        }

        public Day25(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new CodeChronicle(this.Input).Unique()}";

        public string Gold()
            => $"{new CodeChronicle(this.Input).Unique()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new CodeChronicle(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new CodeChronicle(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => CodeChronicleTheme.ToAsciiConfiguration(this);
    }
}
