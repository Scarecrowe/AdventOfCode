namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_08___Two_Factor_Authentication;

    public class Day8 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day8()
            : base(2016, 8, "Two-Factor Authentication")
        {
        }

        public Day8(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new TwoFactorAuthentication(this.Input).Pixels()}";

        public string Gold()
            => $"{new TwoFactorAuthentication(this.Input).Print()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new TwoFactorAuthentication(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new TwoFactorAuthentication(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TwoFactorAuthenticationTheme.ToAsciiConfiguration(this);
    }
}
