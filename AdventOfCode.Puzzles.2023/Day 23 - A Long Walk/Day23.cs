namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_23___A_Long_Walk;

    public class Day23 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day23()
            : base(2023, 23, "A Long Walk")
        {
        }

        public Day23(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ALongWalk(this.Input).LongestHike()}";

        public string Gold()
            => $"{new ALongWalk(this.Input).UniqueLongestHike()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ALongWalk(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ALongWalk(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ALongWalkTheme.ToAsciiConfiguration(this);
    }
}
