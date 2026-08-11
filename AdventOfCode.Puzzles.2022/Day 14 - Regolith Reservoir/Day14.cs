namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_14___Regolith_Reservoir;

    public class Day14 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day14()
            : base(2022, 14, "Regolith Reservoir")
        {
        }

        public Day14(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new RegolithReservoir(this.Input, false).Run().SumOfSand()}";

        public string Gold() => $"{new RegolithReservoir(this.Input, true).Run().SumOfSand()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new RegolithReservoir(this.Input, false, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new RegolithReservoir(this.Input, true, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => RegolithReservoirTheme.ToAsciiConfiguration(this);
    }
}
