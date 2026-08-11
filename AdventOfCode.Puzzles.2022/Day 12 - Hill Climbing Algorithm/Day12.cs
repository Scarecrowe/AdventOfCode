namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_12___Hill_Climbing_Algorithm;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;

    public class Day12 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day12()
            : base(2022, 12, "Hill Climbing Algorithm")
        {
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new HillClimbingAlgorithm(this.Input).Fewest()}";

        public string Gold() => $"{new HillClimbingAlgorithm(this.Input, false).Fewest()}";

        public void SilverFrame(IFrameRenderer renderer)
    => new HillClimbingAlgorithm(this.Input, renderer, true).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new HillClimbingAlgorithm(this.Input, renderer, false).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => HillClimbingAlgorithmTheme.ToAsciiConfiguration(this);
    }
}
