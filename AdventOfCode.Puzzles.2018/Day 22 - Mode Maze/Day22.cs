namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_22___Mode_Maze;

    public class Day22 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day22()
            : base(2018, 22, "Mode Maze")
        {
        }

        public Day22(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ModeMaze(this.Input, 0, 0).BuildMap().CountOfWetAndNarrow()}";

        public string Gold()
            => $"{new ModeMaze(this.Input, 100, 100).BuildMap().WalkMaze()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ModeMaze(this.Input, 0, 0, renderer).BuildMap().RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ModeMaze(this.Input, 100, 100, renderer).BuildMap().RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ModeMazeTheme.ToAsciiConfiguration(this);
    }
}
