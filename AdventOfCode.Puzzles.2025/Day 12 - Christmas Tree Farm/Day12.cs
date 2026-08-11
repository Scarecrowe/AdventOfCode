namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_12___Christmas_Tree_Farm;

    public class Day12 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day12()
            : base(2025, 12, "Christmas Tree Farm")
        {
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ChristmasTreeFarm(this.Input).RegionCount()}";

        public string Gold()
            => string.Empty;

        public void SilverFrame(IFrameRenderer renderer)
            => new ChristmasTreeFarm(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ChristmasTreeFarm(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ChristmasTreeFarmTheme.ToAsciiConfiguration(this);
    }
}
