namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_17___Clumsy_Crucible;

    public class Day17 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day17()
            : base(2023, 17, "Clumsy Crucible")
        {
        }

        public Day17(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ClumsyCrucible(this.Input).CrucibleHeatLoss()}";

        public string Gold()
            => $"{new ClumsyCrucible(this.Input).UltraCrucibleHeatLoss()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ClumsyCrucible(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ClumsyCrucible(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ClumsyCrucibleTheme.ToAsciiConfiguration(this);
    }
}
