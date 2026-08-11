namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_09___Movie_Theater;

    public class Day9 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day9()
            : base(2025, 9, "Movie Theater")
        {
        }

        public Day9(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new MovieTheater(this.Input).AreaOutside()}";

        public string Gold()
            => $"{new MovieTheater(this.Input).AreaInside()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new MovieTheater(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new MovieTheater(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => MovieTheaterTheme.ToAsciiConfiguration(this);
    }
}
