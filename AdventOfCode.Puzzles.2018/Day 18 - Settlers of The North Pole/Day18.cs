namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_18___Settlers_of_The_North_Pole;

    public class Day18 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day18()
            : base(2018, 18, "Settlers of The North Pole")
        {
        }

        public Day18(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new SettlersOfTheNorthPole(this.Input).Cycle(10)}";

        public string Gold()
            => $"{new SettlersOfTheNorthPole(this.Input).Cycle(1000000000)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new SettlersOfTheNorthPole(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new SettlersOfTheNorthPole(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => SettlersOfTheNorthPoleTheme.ToAsciiConfiguration(this);
    }
}
