namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_03___Toboggan_Trajectory;
    using AdventOfCode.Animation.Renderers.Themes;

    public class Day3 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day3()
            : base(2020, 3, "Toboggan Trajectory")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TobogganTrajectory(this.Input).Single()}";

        public string Gold() => $"{new TobogganTrajectory(this.Input).Multiple()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new TobogganTrajectory(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new TobogganTrajectory(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TobogganTrajectoryTheme.ToAsciiConfiguration(this);
    }
}
