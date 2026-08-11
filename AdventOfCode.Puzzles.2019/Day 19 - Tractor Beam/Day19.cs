namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_19___Tractor_Beam;

    public class Day19 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day19()
            : base(2019, 19, "Tractor Beam")
        {
        }

        public Day19(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TractorBeam(this.Input[0]).BuildMap(50, 0, 50).TractorBeamArea}";

        [Slow]
        public string Gold() => $"{new TractorBeam(this.Input[0]).BuildMap(1000, 700, 1200).ClosestPoint()}";


        public void SilverFrame(IFrameRenderer renderer)
            => new TractorBeam(this.Input[0], renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new TractorBeam(this.Input[0], renderer).RenderGold(shipSize: 100, viewportWidth: 220, viewportHeight: 70, renderEvery: 20);

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TractorBeamTheme.ToAsciiConfiguration(this);
    }
}
