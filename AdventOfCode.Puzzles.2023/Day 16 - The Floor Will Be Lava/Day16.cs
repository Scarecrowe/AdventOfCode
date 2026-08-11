namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_16___The_Floor_Will_Be_Lava;

    public class Day16 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day16()
            : base(2023, 16, "The Floor Will Be Lava")
        {
        }

        public Day16(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new TheFloorWillBeLava(this.Input).Beam(new(0, 0), Cardinal.East)}";

        public string Gold()
            => $"{new TheFloorWillBeLava(this.Input).Shine()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new TheFloorWillBeLava(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new TheFloorWillBeLava(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TheFloorWillBeLavaTheme.ToAsciiConfiguration(this);
    }
}
