namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_13___Point_of_Incidence;
    using AdventOfCode.Animation.Renderers.Themes;

    public class Day13 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day13()
            : base(2023, 13, "Point of Incidence")
        {
        }

        public Day13(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{PointOfIncidence.Silver(this.Input)}";

        public string Gold()
            => $"{PointOfIncidence.Gold(this.Input)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new PointOfIncidence(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new PointOfIncidence(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => PointOfIncidenceTheme.ToAsciiConfiguration(this);
    }
}
