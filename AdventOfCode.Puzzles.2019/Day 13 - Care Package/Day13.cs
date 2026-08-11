namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_13___Care_Package;

    public class Day13 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day13()
            : base(2019, 13, "Care Package")
        {
        }

        public Day13(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CarePackage(this.Input[0]).Play().CountBlocks()}";

        public string Gold() => $"{new CarePackage(this.Input[0]).Play(2).Score}";

        public void SilverFrame(IFrameRenderer renderer)
            => new CarePackage(this.Input[0], renderer).RenderGame(1, renderEvery: 4);

        public void GoldFrame(IFrameRenderer renderer)
             => new CarePackage(this.Input[0], renderer).RenderGame(2, renderEvery: 1);

        public IAsciiRendererConfiguration AsciiConfiguration()
            => CarePackageTheme.ToAsciiConfiguration(this);
    }
}
