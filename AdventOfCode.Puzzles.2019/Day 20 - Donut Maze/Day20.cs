namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_20___Donut_Maze;

    public class Day20 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day20()
            : base(2019, 20, "Donut Maze")
        {
        }

        public Day20(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new DonutMaze(this.Input).Search()}";

        public string Gold()
            => $"{new DonutMaze(this.Input).Search(true)}";

        public void SilverFrame(IFrameRenderer renderer)
            => new DonutMaze(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new DonutMaze(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => DonutMazeTheme.ToAsciiConfiguration(this);
    }
}
