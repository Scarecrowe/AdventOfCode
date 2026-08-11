namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_11___Seating_System;
    using AdventOfCode.Animation.Renderers.Themes;

    public class Day11 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day11()
            : base(2020, 11, "Seating System", StringSplitOptions.None)
        {
        }

        public Day11(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SeatingSystem(this.Input).SeatCount()}";

        public string Gold() => $"{new SeatingSystem(this.Input).VisibleSeatCount()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new SeatingSystem(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new SeatingSystem(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => SeatingSystemTheme.ToAsciiConfiguration(this);
    }
}
