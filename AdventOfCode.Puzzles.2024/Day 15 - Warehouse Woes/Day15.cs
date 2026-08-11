namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_15___Warehouse_Woes;

    public class Day15 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day15()
            : base(2024, 15, "Warehouse Woes")
        {
        }

        public Day15(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new WarehouseWoes(this.Input).GpsCoordinate()}";

        public string Gold()
            => $"{new WarehouseWoes(this.Input, true).GpsCoordinate()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new WarehouseWoes(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new WarehouseWoes(this.Input, renderer, true).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => WarehouseWoesTheme.ToAsciiConfiguration(this);
    }
}
