namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_10___Pipe_Maze;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;

    public class Day10 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day10()
            : base(2023, 10, "Pipe Maze")
        {
        }

        public Day10(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new PipeMaze(this.Input).Move()}";

        public string Gold() => $"{new PipeMaze(this.Input).Move()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new PipeMaze(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new PipeMaze(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => PipeMazeTheme.ToAsciiConfiguration(this);
    }
}
