namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_14___Parabolic_Reflector_Dish;

    public class Day14 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day14()
            : base(2023, 14, "Parabolic Reflector Dish")
        {
        }

        public Day14(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ParabolicReflectorDish(this.Input).Silver()}";

        public string Gold()
            => $"{new ParabolicReflectorDish(this.Input).Gold()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ParabolicReflectorDish(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ParabolicReflectorDish(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ParabolicReflectorDishTheme.ToAsciiConfiguration(this);
    }
}
