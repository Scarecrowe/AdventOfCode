namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class Day17 : Puzzle, IPuzzle, IAsciiAnimation, I2dAnimation
    { 
        public Day17()
            : base(2018, 17, "Reservoir Research")
        {
        }

        public Day17(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ReservoirResearch(this.Input).Settle()}";

        public string Gold()
            => $"{new ReservoirResearch(this.Input).Settle(false)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ReservoirResearch(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ReservoirResearch(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ReservoirResearchTheme.ToAsciiConfiguration(this);

        public void Render2DSilver()
             => new ReservoirResearch(this.Input).Animate2D();

        public void Render2DGold()
        {
            throw new NotImplementedException();
        }
    }
}

