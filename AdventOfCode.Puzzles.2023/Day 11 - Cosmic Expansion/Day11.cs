namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_11___Cosmic_Expansion;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;

    public class Day11 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day11()
            : base(2023, 11, "Cosmic Expansion")
        {
        }

        public Day11(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CosmicExpansion(this.Input).SumOfShortestPath(2)}";

        public string Gold() => $"{new CosmicExpansion(this.Input).SumOfShortestPath(1000000)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new CosmicExpansion(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new CosmicExpansion(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => CosmicExpansionTheme.ToAsciiConfiguration(this);
    }
}
