namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_08___Resonant_Collinearity;

    public class Day8 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day8()
            : base(2024, 8, "Resonant Collinearity")
        {
        }

        public Day8(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ResonantCollinearity(this.Input).AntiNodes().Count}";

        public string Gold()
            => $"{new ResonantCollinearity(this.Input).AntiNodes(true).Count}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ResonantCollinearity(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ResonantCollinearity(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ResonantCollinearityTheme.ToAsciiConfiguration(this);
    }
}
