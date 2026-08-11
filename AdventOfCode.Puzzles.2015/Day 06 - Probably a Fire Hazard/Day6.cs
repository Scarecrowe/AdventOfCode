namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_06___Probably_a_Fire_Hazard;

    public class Day6 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day6()
            : base(2015, 6, "Probably a Fire Hazard")
        {
        }

        public Day6(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ProbablyAFireHazard(this.Input, LightBrightness.Single).Lit()}";

        public string Gold()
            => $"{new ProbablyAFireHazard(this.Input, LightBrightness.Multiple).Lit()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ProbablyAFireHazard(this.Input, LightBrightness.Single, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ProbablyAFireHazard(this.Input, LightBrightness.Multiple, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ProbablyAFireHazardTheme.ToAsciiConfiguration(this);
    }
}
