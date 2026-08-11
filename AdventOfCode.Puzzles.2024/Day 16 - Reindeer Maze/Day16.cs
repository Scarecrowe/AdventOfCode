namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_16___Reindeer_Maze;

    public class Day16 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day16()
            : base(2024, 16, "Reindeer Maze")
        {
        }

        public Day16(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new ReindeerMaze(this.Input).BestScore()}";

        public string Gold()
            => $"{new ReindeerMaze(this.Input).Seats()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new ReindeerMaze(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new ReindeerMaze(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => ReindeerMazeTheme.ToAsciiConfiguration(this);
    }
}
