namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_08___Space_Image_Format;
    using AdventOfCode.Animation.Renderers.Themes;

    public class Day8 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day8()
            : base(2019, 8, "Space Image Format")
        {
        }

        public Day8(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SpaceImageFormat(this.Input[0], 25, 6).FewestDigits()}";

        public string Gold() => $"{new SpaceImageFormat(this.Input[0], 25, 6).Decode().Print()}";

        public void SilverFrame(IFrameRenderer renderer) => new SpaceImageFormat(this.Input[0], 25, 6, renderer).Render();

        public void GoldFrame(IFrameRenderer renderer) => throw new NotImplementedException();

        public IAsciiRendererConfiguration AsciiConfiguration() => SpaceImageFormatTheme.ToAsciiConfiguration(this);
    }
}
