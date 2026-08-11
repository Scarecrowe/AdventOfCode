namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_20___Jurassic_Jigsaw;

    public class Day20 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day20()
            : base(2020, 20, "Jurassic Jigsaw", StringSplitOptions.None)
        {
        }

        public Day20(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new JurrassicJigsaw(this.Input).Corners()}";

        public string Gold() => $"{new JurrassicJigsaw(this.Input).NotSeaMonster()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new JurrassicJigsaw(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new JurrassicJigsaw(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => JurassicJigsawTheme.ToAsciiConfiguration(this);
    }
}
