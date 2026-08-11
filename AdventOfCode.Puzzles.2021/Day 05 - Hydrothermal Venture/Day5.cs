namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_05___Hydrothermal_Venture;

    public class Day5 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day5()
            : base(2021, 5, "Hydrothermal Venture")
        {
        }

        public Day5(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new HydrothermalVenture(this.Input, false).TotalCrossOverVents()}";

        public string Gold() => $"{new HydrothermalVenture(this.Input, true).TotalCrossOverVents()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new HydrothermalVenture(this.Input, false, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new HydrothermalVenture(this.Input, true, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => HydrothermalVentureTheme.ToAsciiConfiguration(this);
    }
}
