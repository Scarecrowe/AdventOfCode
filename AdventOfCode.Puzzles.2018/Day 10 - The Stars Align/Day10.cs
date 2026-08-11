namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_10___The_Stars_Align;

    public class Day10 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day10()
            : base(2018, 10, "The Stars Align")

        {
        }

        public Day10(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new TheStarsAlign(this.Input).Align()}";

        public string Gold()
            => $"{new TheStarsAlign(this.Input).Align(false)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new TheStarsAlign(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new TheStarsAlign(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TheStarsAlignTheme.ToAsciiConfiguration(this);
    }
}
