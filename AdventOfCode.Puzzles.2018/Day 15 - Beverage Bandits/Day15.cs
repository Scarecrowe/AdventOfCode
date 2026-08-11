namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_15___Beverage_Bandits;

    public class Day15 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day15()
            : base(2018, 15, "Beverage Bandits")
        {
        }

        public Day15(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new BeverageBandits(this.Input).Battle()}";

        public string Gold()
            => $"{new BeverageBandits(this.Input).BattleWithTechnology()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new BeverageBandits(this.Input, renderer).RenderSilver(renderEveryTurn: 1);

        public void GoldFrame(IFrameRenderer renderer)
            => new BeverageBandits(this.Input, renderer).RenderGold(renderEveryTurn: 1);

        public IAsciiRendererConfiguration AsciiConfiguration()
            => BeverageBanditsTheme.ToAsciiConfiguration(this);
    }
}
